use master;
IF EXISTS(select * from sys.databases where name='library')
	DROP DATABASE libraryProject;

create database libraryProject;

use libraryProject;

create table Publisher	(Id			INT				IDENTITY(1,1)	NOT NULL,
						Name		VARCHAR(200)	NOT NULL,
						CONSTRAINT pk_PUBLISHERS PRIMARY KEY (Id));

create table Book	(Isbn			CHAR(10)			NOT NULL,
					Title			VARCHAR(200)		NOT NULL,
					Cover			VARCHAR(200),
					Pages			INT					NOT NULL,
					PublisherId		INT					NOT NULL,
					IsCheckedOut	BIT					NOT NULL	DEFAULT	0,
					CONSTRAINT pk_BOOK PRIMARY KEY (Isbn),
					CONSTRAINT fk_BOOK_PUBLISHERS		FOREIGN KEY (PublisherId)		REFERENCES Publisher (Id));

create table Authors(Author_id		INT					IDENTITY(1,1)	NOT NULL,
					Name			VARCHAR(200)		NOT NULL,
					CONSTRAINT pk_AUTHOR PRIMARY KEY (Author_id));

create table Book_Authors	(Id			INT			IDENTITY(1,1)	NOT NULL,
							Author_id	INT			NOT NULL,
							Isbn		CHAR(10)	NOT NULL,
							CONSTRAINT pk_BOOK_AUTHORS PRIMARY KEY (Id),
							CONSTRAINT fk_BOOK_AUTHORS_BOOK		FOREIGN KEY (Isbn)		REFERENCES Book (Isbn),
							CONSTRAINT fk_BOOK_AUTHORS_AUTHOR	FOREIGN KEY (Author_id)	REFERENCES Authors (Author_id));

create table Borrower	(Id			int					IDENTITY(1,1)	NOT NULL,
						Card_id		AS	CAST('ID' + RIGHT('000000' + CAST(ID AS VARCHAR(8)), 6) AS CHAR(8))	PERSISTED,
						Ssn			CHAR(10)			NOT NULL,
						Bname		VARCHAR(200)		NOT NULL,
						Address		VARCHAR(200)		NOT NULL,
						Phone		VARCHAR(13)			NOT NULL,
						Email		VARCHAR(60),
						CONSTRAINT pk_BORROWER	PRIMARY KEY (Id),
						CONSTRAINT Ck_BORROWER	UNIQUE (Card_id));

create table Book_Loans	(Loan_id	INT			IDENTITY(1,1)	NOT NULL,
						Isbn		CHAR(10)	NOT NULL,
						Card_id		CHAR(8)		NOT NULL,
						Date_out	DATE		NOT NULL,
						Due_DATE	DATE		NOT NULL,
						Date_in		DATE,
						CONSTRAINT pk_BOOK_LOANS			PRIMARY KEY (Loan_id),
						CONSTRAINT fk_BOOK_LOANS_BOOK		FOREIGN KEY (Isbn)		REFERENCES Book (Isbn),
						CONSTRAINT fk_BOOK_LOANS_BORROWER	FOREIGN KEY (Card_id)	REFERENCES Borrower (Card_id));

create table Fines	(Loan_id		INT				NOT NULL,
					Fine_amt		DECIMAL(8,2)	NOT NULL,
					Paid			BIT				NOT NULL,
					CONSTRAINT pk_FINES				PRIMARY KEY (Loan_id),
					CONSTRAINT fk_FINES_BOOK_LOANS	FOREIGN KEY (Loan_id) REFERENCES BOOK_LOANS (Loan_id));