create database BIKEMGMTSYSTEM

go

use BIKEMGMTSYSTEM

go


create table Menu(
[idMenu] int primary key identity(1,1),
[description] varchar(30),
[idMenuParent] int references Menu(idMenu),
[icon] varchar(30),
[controller] varchar(30),
[pageAction] varchar(30),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go


create table Rol(
[idRol] int primary key identity(1,1),
[description] varchar(30),
[isActive] bit,
[registrationDate] datetime default getdate()
)
 go
 
 create table RolMenu(
 [idRolMenu] int primary key identity(1,1),
 [idRol] int references Rol(idRol),
 [idMenu] int references Menu(idMenu),
 [isActive] bit,
 [registrationDate] datetime default getdate()
 )

 go


create table Users(
[idUsers] int primary key identity(1,1),
[name] varchar(50),
[email] varchar(80),
[phone] varchar(50),
[idRol] int references Rol(idRol),
[password] varchar(100),
[photo] varbinary(max),
[cnicFront] varbinary(max),
[cnicBack] varbinary(max),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go

create table Bike(
[idBike] int primary key identity(1,1),
[name] varchar(50),
[brand] varchar(50),
[price/hour] decimal(10,2),
[photo] varbinary(max),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go

create table Payment(
[idPayment] int primary key identity(1,1),
[name] varchar(50),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go

create table Invoice(
[idInvoice] int primary key identity(1,1),
[idUser] int references Users(idUsers),
[idBike] int references Bike(idBike),
[idPayment] int references Payment(idPayment),
totalHours float,
[totalAmount] float,
[invoiceTime] time default getdate()
)





