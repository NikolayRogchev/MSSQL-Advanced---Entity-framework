CREATE TABLE Minions(
Id int primary key identity,
Name nvarchar(50) not null,
Age int check(Age >= 0),
TownId int not null
)
CREATE TABLE Towns(
Id int primary key identity,
Name nvarchar(50) not null,
CountryId int not null
)
CREATE TABLE Villains(
Id int primary key identity,
Name nvarchar(50) not null,
EvilnessFactor varchar(20) check(EvilnessFactor in ('Good', 'Bad', 'Evil', 'Super Evil'))
)
CREATE TABLE Countries(
Id int primary key identity,
CountryName nvarchar(50) not null
)
CREATE TABLE MinionsVillains(
MinionId int not null,
VillainId int not null
)

ALTER TABLE Minions
ADD CONSTRAINT fk_Minions_Towns FOREIGN KEY (TownId) REFERENCES Towns(Id)

ALTER TABLE Towns
ADD CONSTRAINT fk_Towns_Countries FOREIGN KEY (CountryId) REFERENCES Countries(Id)

ALTER TABLE MinionsVillains
ADD CONSTRAINT fk_MinionsVillains_Minions FOREIGN KEY (MinionId) REFERENCES Minions(Id),
	CONSTRAINT fk_MinionsVillains_Villains FOREIGN KEY (VillainId) REFERENCES Villains(Id),
	CONSTRAINT pk_MinionsVillains PRIMARY KEY (MinionId, VillainId)