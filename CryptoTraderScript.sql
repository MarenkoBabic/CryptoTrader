
CREATE TABLE Person(
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	email VARCHAR(128) NOT NULL,
	password CHAR(64) NOT NULL,
	salt CHAR(64) NOT NULL,
	firstName VARCHAR(64) NOT NULL,
	lastName VARCHAR(64) NOT NULL,
	role varchar(6) NOT NULL DEFAULT 'User',
	active BIT NOT NULL DEFAULT 1,
	status BIT NOT NULL DEFAULT 0,
	reference VARCHAR(40) NULL,
	CONSTRAINT pk_Person PRIMARY KEY(id),
	INDEX ix_name(lastname, firstname)
);
GO

CREATE UNIQUE INDEX ui_Person_email ON Person(email);
GO

CREATE TABLE Country (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	countryName VARCHAR(128),
	iso CHAR(3),
	CONSTRAINT pk_Country PRIMARY KEY(id)
);
GO

CREATE UNIQUE INDEX ui_Country_name ON Country(countryName);
GO

CREATE UNIQUE INDEX ui_Country_iso ON Country(iso);
GO

CREATE TABLE City (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	zip VARCHAR(8) NOT NULL,
	cityName VARCHAR(64) NOT NULL,
	country_id INT NOT NULL,
	CONSTRAINT pk_City PRIMARY KEY(id),
	CONSTRAINT fk_City_Country FOREIGN KEY(country_id) REFERENCES Country(id)
);
GO

CREATE TABLE Address (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	person_id INT NOT NULL,
	street VARCHAR(128) NOT NULL,
	number VARCHAR(16) NOT NULL,
	city_id INT NOT NULL,
	CONSTRAINT pk_Address PRIMARY KEY(id),
	CONSTRAINT fk_Address_Person FOREIGN KEY(person_id) REFERENCES Person(id),
	CONSTRAINT fk_Address_City FOREIGN KEY(city_id) REFERENCES City(id)
);
GO

CREATE UNIQUE INDEX ui_Address_person_id ON Address(person_id);
GO

CREATE TABLE Upload (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	person_id INT NOT NULL,
	path VARCHAR(255) NOT NULL,
	CONSTRAINT pk_Upload PRIMARY KEY(id),
	CONSTRAINT fk_Upload_User FOREIGN KEY(person_id) REFERENCES Person(id)
);
GO

CREATE UNIQUE INDEX ui_Upload_user_id ON Upload(person_id);
GO

CREATE TABLE BankAccount (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	person_id INT NOT NULL,
	iban VARCHAR(34) NOT NULL,
	bic VARCHAR(11) NOT NULL,
	CONSTRAINT pk_BankAccount PRIMARY KEY(id),
	CONSTRAINT fk_BankAccount_User FOREIGN KEY(person_id) REFERENCES Person(id)
);
GO

CREATE UNIQUE INDEX ui_BankAccount_user_id ON BankAccount(person_id);
GO

CREATE UNIQUE INDEX ui_BankAccount_iban_bic ON BankAccount(iban, bic);
GO

CREATE TABLE Balance (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	person_id INT NOT NULL,
	currency CHAR(5) NOT NULL,
	amount DECIMAL(16, 8) NOT NULL,
	CONSTRAINT pk_Balance PRIMARY KEY(id),
	CONSTRAINT fk_Balance_User FOREIGN KEY(person_id) REFERENCES Person(id),
	INDEX ix_person_id(person_id)
);
GO

CREATE TABLE BankTransferHistory (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	person_id INT NOT NULL,
	amount DECIMAL(16, 8) NOT NULL,
	currency CHAR(5) NOT NULL,
	CONSTRAINT pk_BankTransferHistory PRIMARY KEY(id),
	CONSTRAINT fk_BankTransferHistory_User FOREIGN KEY(person_id) REFERENCES Person(id),
	INDEX ix_created(created),
	INDEX ix_person_id(person_id)
);
GO

CREATE TABLE Ticker (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	rate DECIMAL(16, 8) NOT NULL,
	currency_src CHAR(5) NOT NULL,
	currency_trg CHAR(5) NOT NULL,
	CONSTRAINT pk_Ticker PRIMARY KEY(id),
	INDEX ix_created(created)
);
GO

CREATE TABLE TradeHistory (
	id INT NOT NULL IDENTITY,
	created DATETIME NOT NULL DEFAULT GETDATE(),
	person_id INT NOT NULL,
	amount DECIMAL(16, 8) NOT NULL,
	ticker_id INT NOT NULL,
	CONSTRAINT pk_TradeHistory PRIMARY KEY(id),
	CONSTRAINT fk_TradeHistory_User FOREIGN KEY(person_id) REFERENCES Person(id),
	CONSTRAINT fk_TradeHistory_Ticker FOREIGN KEY(ticker_id) REFERENCES Ticker(id),
	INDEX ix_created(created),
	INDEX ix_user_id(person_id)
);
GO
