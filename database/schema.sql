-- ==========================================
-- 1. RESTAURANTS TABLE
-- ==========================================
create table public.restaurants (
    id bigint generated always as identity primary key,
    name text not null,
    slug text not null unique,
    category text not null,
    neighborhood text not null,
    distance text not null,
    rating numeric(3,1) not null default 5.0 check (rating >= 0 and rating <= 5),
    review_count int not null default 0 check (review_count >= 0),
    hours text not null,
    banner_image_url text not null,
    created_at timestamptz not null default now()
);

create index idx_restaurants_name on public.restaurants (name);
create index idx_restaurants_slug on public.restaurants (slug);

-- ==========================================
-- 2. DISHES TABLE (Foreign Key + Soft Delete)
-- ==========================================
create table public.dishes (
    id bigint generated always as identity primary key,
    restaurant_id bigint not null references public.restaurants(id) on delete cascade,
    name text not null,
    description text not null,
    price numeric(10,2) not null default 0 check (price >= 0),
    category text not null default 'Mains',
    image_url text not null,
    uses_dedicated_fryer boolean not null default false,
    has_dedicated_prep_table boolean not null default false,
    confirmed_notes text not null default '',
    ingredients_spec text not null default '',
    modifications_csv text not null default '',
    deleted_at timestamptz,
    created_at timestamptz not null default now()
);

create index idx_dishes_name on public.dishes (name);
create index idx_dishes_restaurant_id on public.dishes (restaurant_id);

-- ==========================================
-- 3. KITCHEN AUDITS TABLE (Foreign Key + Soft Delete)
-- ==========================================
create table public.kitchen_audits (
    id bigint generated always as identity primary key,
    restaurant_id bigint not null references public.restaurants(id) on delete cascade,
    restaurant_name text not null,
    comment text not null,
    tags_csv text not null default '',
    upvotes int not null default 0 check (upvotes >= 0),
    deleted_at timestamptz,
    created_at timestamptz not null default now()
);

create index idx_audits_restaurant_id on public.kitchen_audits (restaurant_id);

-- ==========================================
-- 4. SECURITY LOCKS (GRANT & RLS - Class Policies)
-- ==========================================
grant select, insert, update, delete on public.restaurants, public.dishes, public.kitchen_audits to anon;

alter table public.restaurants enable row level security;
alter table public.dishes enable row level security;
alter table public.kitchen_audits enable row level security;

-- Restaurants policies
create policy "read_restaurants" on public.restaurants for select to anon using (true);
create policy "insert_restaurants" on public.restaurants for insert to anon with check (true);
create policy "update_restaurants" on public.restaurants for update to anon using (true) with check (true);

-- Dishes policies
create policy "read_dishes" on public.dishes for select to anon using (true);
create policy "insert_dishes" on public.dishes for insert to anon with check (true);
create policy "update_dishes" on public.dishes for update to anon using (true) with check (true);
create policy "delete_dishes" on public.dishes for delete to anon using (true);

-- Audits policies
create policy "read_audits" on public.kitchen_audits for select to anon using (true);
create policy "insert_audits" on public.kitchen_audits for insert to anon with check (true);
create policy "update_audits" on public.kitchen_audits for update to anon using (true) with check (true);
create policy "delete_audits" on public.kitchen_audits for delete to anon using (true);

-- ==========================================
-- 5. SEED INITIAL DATA
-- ==========================================
insert into public.restaurants (name, slug, category, neighborhood, distance, rating, review_count, hours, banner_image_url)
values
('Artisan Kitchen', 'artisan-kitchen', 'Seasonal American', 'Williamsburg', '0.8 mi', 4.9, 128, 'Open until 10 pm', 'https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?auto=format&fit=crop&w=1600&q=80'),
('Green Fork Bistro', 'green-fork-bistro', 'Farm-to-Table & Vegan', 'Greenpoint', '1.2 mi', 4.8, 95, 'Open until 9 pm', 'https://images.unsplash.com/photo-1555396273-367ea4eb4db5?auto=format&fit=crop&w=1600&q=80');

insert into public.dishes (restaurant_id, name, description, price, category, image_url, uses_dedicated_fryer, has_dedicated_prep_table, confirmed_notes, ingredients_spec, modifications_csv)
values
(1, 'Truffle Polenta', 'Golden corn polenta, wild mushrooms & truffle olive oil.', 18.00, 'Starters', 'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=600&q=80', true, true, 'No gluten or dairy ingredients. Separate prep table confirmed.', 'Cornmeal|None;Mushrooms|None;Olive Oil|None', ''),
(1, 'Mushroom Risotto', 'Arborio rice, forest mushrooms & house vegetable stock.', 22.00, 'Mains', 'https://images.unsplash.com/photo-1633964913295-ceb43826e7c9?auto=format&fit=crop&w=600&q=80', true, true, '', 'Arborio Rice|None;Butter|Dairy;Parmesan|Dairy', 'Omit butter;Omit parmesan'),
(1, 'Seafood Linguine', 'Fresh linguine, shrimp & mussels in a cream sauce.', 26.00, 'Mains', 'https://images.unsplash.com/photo-1563379091339-03b21ab4a4f8?auto=format&fit=crop&w=600&q=80', false, false, '', 'Fresh Linguine|Gluten;Shrimp|Shellfish;Heavy Cream|Dairy', ''),
(2, 'Quinoa Harvest Bowl', 'Roasted sweet potatoes, avocado, quinoa, and tahini drizzle.', 16.00, 'Starters', 'https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=600&q=80', true, true, '100% naturally gluten-free and dairy-free.', 'Quinoa|None;Avocado|None', '');

insert into public.kitchen_audits (restaurant_id, restaurant_name, comment, tags_csv, upvotes)
values
(1, 'Artisan Kitchen', 'Manager came out personally to double-check their dedicated corn fryer. Zero gluten reaction, amazing experience!', 'Celiac,Gluten-Free,Shellfish', 42),
(2, 'Green Fork Bistro', 'Completely vegan prep line. No butter or dairy cross-contamination whatsoever. Super allergy-friendly staff.', 'Dairy-Free,Vegan', 28);