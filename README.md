# BuildingServicesReservation

to create local database first use:


-- tables
-- Table: Oferta
CREATE TABLE Oferta (
    idOferta int NOT NULL,
    Nazwa varchar(150)  NOT NULL,
    Opis varchar(500)  NOT NULL,
    Dniowka decimal(8,2)  NOT NULL,
    idUslugodawca int  NOT NULL,
    CONSTRAINT Oferta_pk PRIMARY KEY  (idOferta)
);

-- Table: Opinie
CREATE TABLE Opinia (
    idOpinia int  NOT NULL,
    Opis varchar(500)  NOT NULL,
    Ocena int  NOT NULL,
    idUslugodawca int  NOT NULL,
    idUslugobiorca int  NOT NULL,
    CONSTRAINT Opinia_pk PRIMARY KEY  (idOpinia)
);

-- Table: Pracownik
CREATE TABLE Pracownik (
    idPracownik int  NOT NULL,
    Imie varchar(50)  NOT NULL,
    Nazwisko varchar(50)  NOT NULL,
    CONSTRAINT Pracownik_pk PRIMARY KEY  (idPracownik)
);

-- Table: Rezerwacja
CREATE TABLE Rezerwacja (
    idRezerwacja int  NOT NULL,
    DataOd date  NOT NULL,
    DataDo date  NOT NULL,
    Koszt numeric(10,2)  NOT NULL,
    Status varchar(100)  NOT NULL,
    CzyZaplacone bit NOT NULL,
    idUslugobiorca int  NOT NULL,
    idOferta int  NOT NULL,
    CONSTRAINT Rezerwacja_pk PRIMARY KEY  (idRezerwacja)
);

-- Table: User
CREATE TABLE "User" (
    idUser int  NOT NULL,
    Login varchar(50)  NOT NULL,
    Haslo varchar(50)  NOT NULL,
    idUslugodawca int  NULL,
    idUslugobiorca int  NULL,
    CONSTRAINT User_pk PRIMARY KEY  (idUser)
);

-- Table: Uslugobiorca
CREATE TABLE Uslugobiorca (
    idUslugobiorca int  NOT NULL,
    Imie varchar(50)  NOT NULL,
    Nazwisko varchar(50)  NOT NULL,
    CONSTRAINT Uslugobiorca_pk PRIMARY KEY  (idUslugobiorca)
);

-- Table: Uslugodawca
CREATE TABLE Uslugodawca (
    idUslugodawca int  NOT NULL,
    Nazwa varchar(100)  NOT NULL,
    NIP varchar(20)  NOT NULL,
    CONSTRAINT Uslugodawca_pk PRIMARY KEY  (idUslugodawca)
);

-- Table: Uslugodawca_Pracownik
CREATE TABLE Uslugodawca_Pracownik (
    idUslugodawca_Pracownik int  NOT NULL,
    Stanowisko varchar(150)  NOT NULL,
    idPracownik int  NOT NULL,
    idUslugodawca int  NOT NULL,
    CONSTRAINT Uslugodawca_Pracownik_pk PRIMARY KEY  (idUslugodawca_Pracownik)
);

-- Table: Zdjecie
CREATE TABLE Zdjecie (
    idZdjecie int  NOT NULL,
    NazwaPliku varchar(200)  NOT NULL,
    idUslugodawca int  NOT NULL,
    CONSTRAINT Zdjecie_pk PRIMARY KEY  (idZdjecie)
);

-- foreign keys
-- Reference: Oferta_Uslugodawca (table: Oferta)
ALTER TABLE Oferta ADD CONSTRAINT Oferta_Uslugodawca
    FOREIGN KEY (idUslugodawca)
    REFERENCES Uslugodawca (idUslugodawca);

-- Reference: Opinie_Uslugobiorca (table: Opinie)
ALTER TABLE Opinie ADD CONSTRAINT Opinie_Uslugobiorca
    FOREIGN KEY (idUslugobiorca)
    REFERENCES Uslugobiorca (idUslugobiorca);

-- Reference: Opinie_Uslugodawca (table: Opinie)
ALTER TABLE Opinie ADD CONSTRAINT Opinie_Uslugodawca
    FOREIGN KEY (idUslugodawca)
    REFERENCES Uslugodawca (idUslugodawca);

-- Reference: Rezerwacja_Oferta (table: Rezerwacja)
ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Oferta
    FOREIGN KEY (idOferta)
    REFERENCES Oferta (idOferta);

-- Reference: Rezerwacja_Uslugobiorca (table: Rezerwacja)
ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Uslugobiorca
    FOREIGN KEY (idUslugobiorca)
    REFERENCES Uslugobiorca (idUslugobiorca);

-- Reference: User_Uslugobiorca (table: User)
ALTER TABLE "User" ADD CONSTRAINT User_Uslugobiorca
    FOREIGN KEY (idUslugobiorca)
    REFERENCES Uslugobiorca (idUslugobiorca);

-- Reference: User_Uslugodawca (table: User)
ALTER TABLE "User" ADD CONSTRAINT User_Uslugodawca
    FOREIGN KEY (idUslugodawca)
    REFERENCES Uslugodawca (idUslugodawca);

-- Reference: Uslugodawca_Pracownik_Pracownik (table: Uslugodawca_Pracownik)
ALTER TABLE Uslugodawca_Pracownik ADD CONSTRAINT Uslugodawca_Pracownik_Pracownik
    FOREIGN KEY (idPracownik)
    REFERENCES Pracownik (idPracownik);

-- Reference: Uslugodawca_Pracownik_Uslugodawca (table: Uslugodawca_Pracownik)
ALTER TABLE Uslugodawca_Pracownik ADD CONSTRAINT Uslugodawca_Pracownik_Uslugodawca
    FOREIGN KEY (idUslugodawca)
    REFERENCES Uslugodawca (idUslugodawca);

-- Reference: Zdjecie_Uslugodawca (table: Zdjecie)
ALTER TABLE Zdjecie ADD CONSTRAINT Zdjecie_Uslugodawca
    FOREIGN KEY (idUslugodawca)
    REFERENCES Uslugodawca (idUslugodawca);

-- Update table name
EXEC sp_rename 'Opinie', 'Opinia';


-- End of file.

