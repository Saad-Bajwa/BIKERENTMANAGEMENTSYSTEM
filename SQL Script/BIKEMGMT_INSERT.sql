
--________________________________ INSERT ROL ________________________________
insert into rol([description],isActive) values
('Admin',1),
('User',1)

go


--________________________________ INSERT USER DEFAULT ________________________________

insert into Users(name,email,phone,idRol,[password],photo,isActive) values
('codeStudent','codeStudent@example.com','909090',1,'123',null,1)

go

--________________________________ INSERT MENUS ________________________________

--*menu parent

insert into Menu([description],isActive) values
('Admin',1),
('Inventory',1),
('Order',1),
('Reports',1)


--*menu child - Admin
insert into Menu([description],idMenuParent,controller,pageAction,isActive) values
('DashBoard',1,'Admin','DashBoard',1),
('Users',1,'Admin','Users',1),
('Account History',1,'Admin','AccountHistory',1)


--*menu child - Inventory
insert into Menu([description],idMenuParent,controller,pageAction,isActive) values
('Bikes',2,'Inventory','Bikes',1)

--*menu child - Sales
insert into Menu([description],idMenuParent,controller,pageAction,isActive) values
('New Order',3,'Order','NewOrder',1)


UPDATE Menu SET idMenuParent = idMenu where idMenuParent is null


go
--________________________________ INSERT MENU ROLE ________________________________


--*Admin
INSERT INTO RolMenu(idRol,idMenu,isActive) values
(1,1,1),
(1,2,1),
(1,3,1),
(1,4,1),
(1,5,1),
(1,6,1),
(1,7,1),
(1,8,1),
(1,9,1)

--*User
INSERT INTO RolMenu(idRol,idMenu,isActive) values
(2,3,1),
(2,9,1)


go
--________________________________ INSERT Payment Type ________________________________

INSERT INTO Payment(name,isActive) values
('By Cash',1),
('By Credit Card',1)
