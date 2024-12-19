create table customers (
                           id int primary key,
                           name varchar(255) not null,
                           phonenumber varchar(15) not null
);

create table tables (
                        id int primary key,
                        tablenumber int not null,
                        isoccupied boolean not null default false
);

create table menu_items (
                            id int primary key,
                            name varchar(255) not null,
                            price decimal(10, 2) not null,
                            category varchar(100) not null
);

create table orders (
                        id int primary key,
                        customerid int references customers(id),
                        tableid int references tables(id),
                        status varchar(50) not null
);

create table orderitems (
                             id int primary key,
                             orderid int references orders(id),
                             menu_items int references menu_items(id)
);
