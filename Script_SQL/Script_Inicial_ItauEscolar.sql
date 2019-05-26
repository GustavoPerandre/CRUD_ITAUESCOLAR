--Executar este script para o total funcionamento do sistema ItauEscolar
--Massa de dados Opcional

CREATE DATABASE escola;
GO

 USE escola;
 GO

DROP TABLE IF EXISTS dbo.Quadro;
DROP TABLE IF EXISTS dbo.Disciplina;
DROP TABLE IF EXISTS dbo.Professor;
GO  

 CREATE TABLE Disciplina
 (
	disciplina_id int not null identity(1,1) primary key,
	titulo varchar(50) not null UNIQUE
 )

GO

 CREATE TABLE Professor
 (
	professor_id int not null identity(1,1) primary key,
	nome varchar(50) not null UNIQUE
 )

GO

 CREATE TABLE Quadro
 (
	quadro_id int not null identity(1,1) primary key,
	fk_professor_id int not null,
	fk_disciplina_id int not null
 )

GO

ALTER TABLE Quadro
 add constraint FK_Disciplina_Quadro Foreign Key (fk_disciplina_id) references Disciplina (disciplina_id)

 go

 Alter table Quadro
 add constraint FK_Professor_Quadro Foreign Key (fk_professor_id) references Professor (professor_id)

 GO


INSERT INTO Professor VALUES ('Gustavo Reis Perandré')
INSERT INTO Professor VALUES ('Alice Koyama')
INSERT INTO Professor VALUES ('Paulo Roberto')
INSERT INTO Professor VALUES ('Osvaldo Cruz')
INSERT INTO Professor VALUES ('Telésforo Cáceres')
GO

INSERT INTO Disciplina VALUES ('Português')
INSERT INTO Disciplina VALUES ('Matemática')
INSERT INTO Disciplina VALUES ('Inglês')
INSERT INTO Disciplina VALUES ('Geografia')
INSERT INTO Disciplina VALUES ('História')
INSERT INTO Disciplina VALUES ('Artes')
INSERT INTO Disciplina VALUES ('Literatura')
GO

INSERT INTO Quadro VALUES (1, 1)
INSERT INTO Quadro VALUES (1, 2)
INSERT INTO Quadro VALUES (2, 7)
INSERT INTO Quadro VALUES (2, 6)
INSERT INTO Quadro VALUES (3, 5)
INSERT INTO Quadro VALUES (3, 1)
INSERT INTO Quadro VALUES (5, 4)
GO