CREATE TABLE [dbo].[Uzivatel] (
    [UzivatelskeJmeno] VARCHAR (50)   NOT NULL,
    [HesloHash]        VARCHAR (1000) NOT NULL,
    [EmailovaAdresa]   VARCHAR (50)   NOT NULL,
    [Sul]              VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([UzivatelskeJmeno] ASC)
);

