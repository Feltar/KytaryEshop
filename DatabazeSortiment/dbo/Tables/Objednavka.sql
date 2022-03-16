CREATE TABLE [dbo].[Objednavka] (
    [Datum]        DATE           NOT NULL,
    [IdObjednavka] INT            IDENTITY (1, 1) NOT NULL,
    [IdUzivatel]   NVARCHAR (450) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdObjednavka] ASC)
);

