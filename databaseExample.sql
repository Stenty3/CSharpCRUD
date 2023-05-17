create database users
use users
create table usuarios(
    idClient int identity(1,1) not null primary key,
    nombre varchar(250) not null,
    contra varchar(250) not null
)
create table carro(
    idCarro int identity(1,1) not null primary key, -- Se agrega "identity(1,1)"
    idClient int not null,
    color varchar(250) not null,
    foreign key(idClient) references usuarios(idClient)
)
create table garantia(
    idCarro int not null, -- Se elimina "primary key"
    diasGarantia int not null,
    foreign key(idCarro) references carro(idCarro)
)

insert into usuarios(nombre, contra) 
values('camilo', 'hola'), ('Laura', 'nose'), ('Juan', 'Jose');

insert into carro (idClient, color)
values (1, 'rojo'), (2, 'azul'), (3, 'verde');

insert into garantia (idCarro, diasGarantia)
values (1, 20), (2, 40), (3, 342);

select * 
from usuarios
inner join carro on usuarios.idClient = carro.idClient
inner join garantia on carro.idCarro = garantia.idCarro;
