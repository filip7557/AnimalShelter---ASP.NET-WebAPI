create table "User" (
	"Id" SERIAL primary key,
	"Email" VARCHAR(255) not null unique,
	"Password" VARCHAR(255) not null,
	"Name" VARCHAR(255) not null,
	"Address" VARCHAR(255)
);

create table "Restaurant" (
	"Id" SERIAL primary key,
	"Name" VARCHAR(255) not null,
	"Address" VARCHAR(255) not null
);

create table "Deliverer" (
	"Id" SERIAL primary key,
	"Email" VARCHAR(255) not null unique,
	"Password" VARCHAR(255) not null,
	"Name" VARCHAR(255) not null
);

create table "Item" (
	"Id" SERIAL primary key,
	"Name" VARCHAR(255) not null
);

create table "Order" (
	"Id" SERIAL primary key,
	"UserId" INT not null,
	"RestaurantId" INT not null,
	"DelivererId" INT not null,
	"TotalPrice" DECIMAL not null,
	"CreatedAt" TIMESTAMP not null,
	"Status" VARCHAR(50),
	constraint "FK_Order_User_UserId" foreign key ("UserId") references "User"("Id"),
	constraint "FK_Order_Restaurant_RestaurantId" foreign key ("RestaurantId") references "Restaurant"("Id"),
	constraint "FK_Order_Deliverer_DelivererId" foreign key ("DelivererId") references "Deliverer"("Id")
);

create table "RestaurantItem" (
	"Id" SERIAL primary key,
	"RestaurantId" INT not null,
	"ItemId" INT not null,
	"Price" DECIMAL not null,
	constraint "FK_RestaurantItem_Restaurant_RestaurantId" foreign key ("RestaurantId") references "Restaurant"("Id"),
	constraint "FK_RestaurantItem_Item_ItemId" foreign key ("ItemId") references "Item"("Id")
);

create table "OrderRestaurantItem" (
	"Id" SERIAL primary key,
	"OrderId" INT not null,
	"RestaurantItemId" INT not null,
	constraint "FK_OrderRestaurantItem_Order_OrderId" foreign key ("OrderId") references "Order"("Id"),
	constraint "FK_OrderRestaurantItem_RestaurantItem_RestaurantItemId" foreign key ("RestaurantItemId") references "RestaurantItem"("Id")
);

INSERT INTO "User" ("Email", "Password", "Name", "Address")
VALUES 
('ivan.horvat@example.com', 'password123', 'Ivan Horvat', 'Ulica grada Vukovara 13, Osijek'),
('luka.horvat@example.com', 'securepass456', 'Luka Horvat', 'Ulica cara Hadrijana 45, Osijek'),
('filip.horvat@example.com', 'mypassword789', 'Filip Horvat', 'Sjenjak 103, Osijek'),
('sara.horvat@example.com', 'strongpass001', 'Sara Horvat', 'Vijenac Ivana Meštrovića 1, Osijek'),
('ema.horvat@example.com', 'horvat2025!', 'Ema Horvat', 'Drinska ulica 23, Osijek');

INSERT INTO "Restaurant" ("Name", "Address")
VALUES
('Pizza Hut', 'Ulica majstora pizze 3, Osijek'),
('McDonalds', 'Drinska ulica 1, OSijek'),
('Sushi Spot', 'Morska ulica 41, Osijek'),
('Taco Town', 'Ulica grada Vukovara 115, Osijek'),
('Pasta Place', 'Ulica cara Hadrijana 52, Osijek');

INSERT INTO "Deliverer" ("Email", "Password", "Name")
VALUES
('delivery.ivan@example.com', 'pass123', 'Delivery Ivan'),
('delivery.jana@example.com', 'pass456', 'Delivery Jana'),
('delivery.filip@example.com', 'pass789', 'Delivery Filip'),
('delivery.sara@example.com', 'password123', 'Delivery Sara'),
('delivery.ema@example.com', 'delivery!', 'Delivery Ema');

INSERT INTO "Item" ("Name")
VALUES
('Margherita Pizza'),
('Cheeseburger'),
('Slavonska Pizza'),
('Sushi'),
('Spaghetti Carbonara');

INSERT INTO "Order" ("UserId", "RestaurantId", "DelivererId", "TotalPrice", "CreatedAt", "Status")
VALUES
(1, 1, 1, 19.99, CURRENT_TIMESTAMP, 'Completed'),
(2, 2, 2, 15.50, CURRENT_TIMESTAMP, 'Pending'),
(3, 3, 3, 22.75, CURRENT_TIMESTAMP, 'In Progress'),
(4, 4, 4, 9.99, CURRENT_TIMESTAMP, 'Delivered'),
(5, 5, 5, 18.00, CURRENT_TIMESTAMP, 'Cancelled');

INSERT INTO "RestaurantItem" ("RestaurantId", "ItemId", "Price")
VALUES
(1, 1, 9.99),
(2, 2, 7.99),
(3, 3, 12.50),
(4, 4, 3.99),
(5, 5, 10.99);

INSERT INTO "OrderRestaurantItem" ("OrderId", "RestaurantItemId")
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

select "Name"
from "Item"
where "Id" = 2;

select "User"."Name" as "UserName", "Deliverer"."Name" as "DelivererName", "User"."Address" as "DeliveryAddress", "Restaurant"."Name" as "RestaurantName", "TotalPrice", "Status"
from "User"
join "Order" on "User"."Id" = "Order"."UserId"
join "Deliverer" on "Order"."DelivererId" = "Deliverer"."Id"
join "Restaurant" on "Order"."RestaurantId" = "Restaurant"."Id";

update "User"
set "Name" = 'Jana Horvat'
where "Id" = 4;

delete from "Restaurant" 
where "Id" = 4;




