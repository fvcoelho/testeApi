/*	insert into 
Country
States
Cities
*/

insert into Country (cnt_country) values ('Brasil')
insert into Country (cnt_country) values ('Canada')
insert into Country (cnt_country) values ('EUA')
insert into Country (cnt_country) values ('Mexico')

insert into States 
	(sta_name,
	sta_abbreviation,
	sta_regionId,
	sta_country)
values ('Sao Paulo', 'SP', 1, 'Brasil')

insert into States 
	(sta_name,
	sta_abbreviation,
	sta_regionId,
	sta_country)
values ('Rio Janeiro', 'RJ', 2, 'Brasil')

insert into States 
	(sta_name,
	sta_abbreviation,
	sta_regionId,
	sta_country)
values ('Goias', 'GO', 3, 'Brasil')

insert into Cities 
	(cit_name,
	cit_stateId,
	cit_isCapital)
values ('Osasco', 1, 0)

insert into Cities 
	(cit_name,
	cit_stateId,
	cit_isCapital)
values ('Santos', 1, 0)

insert into Cities 
	(cit_name,
	cit_stateId,
	cit_isCapital)
values ('Ubatuba', 1, 0)
