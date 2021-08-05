# Konzolska-aplikacija-1
Ovo je jednostavne konzolske aplikacije koja demonstrira osnovne operacije sa tabelom u SQL Serveru kao sto su listanje i prikazivanje tabele, unos novog zapisa, brisanje postojeceg, trazenje i izmena.

Da bi ona mogla uspesno raditi, potrebno je da na SQL Serveru, na lokalnoj masini, postoji tabela koja se zove "tabImenik". Kod za njeno kreiranje je:

CREATE TABLE [dbo].[tabImenik] (
    [chrIme]     CHAR (20) NOT NULL,
    [chrTelefon] CHAR (30) NULL,
    [chrAdresa]  CHAR (50) NULL,
    CONSTRAINT [PK_tabImenik] PRIMARY KEY CLUSTERED ([chrIme] ASC)
