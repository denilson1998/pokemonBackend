

create database pokemonsDb;
use pokemonsDb;

create table pokemons(
id int primary key identity,
name varchar(30),
season varchar(30),
partner varchar(100),
imageUrl varchar(500)
);


