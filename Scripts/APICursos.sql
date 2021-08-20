IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'cursos')
	BEGIN
		CREATE DATABASE cursos
	END
GO 

USE cursos

GO

IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'aluno')
	BEGIN
		CREATE TABLE aluno (
			cod_matricula INT IDENTITY(1,1),
			nome varchar(60),
			cod_turma INT NOT NULL
		);
	END


IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'turma')
	BEGIN
		CREATE TABLE turma (
			cod_turma INT IDENTITY(1,1),
			idioma_turma varchar(60),
			data_inicio DATE
		);
	END

IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'usuario')
	BEGIN
		CREATE TABLE usuario (
			nome_usuario varchar(60) NOT NULL,
			senha varchar(60)

		);
	END

GO

ALTER TABLE aluno
ADD CONSTRAINT Pk_aluno_cod_matricula PRIMARY KEY(cod_matricula)

ALTER TABLE turma
ADD CONSTRAINT Pk_turma_cod_turma PRIMARY KEY(cod_turma)

ALTER TABLE usuario
ADD CONSTRAINT Pk_usuario_nome_usuario PRIMARY KEY(nome_usuario)

ALTER TABLE aluno
ADD CONSTRAINT Fk_aluno_turma FOREIGN KEY (cod_turma)
REFERENCES turma(cod_turma)

GO




