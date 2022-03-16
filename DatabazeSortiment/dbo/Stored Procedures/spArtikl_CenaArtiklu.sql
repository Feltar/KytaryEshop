CREATE PROCEDURE [dbo].[spArtikl_CenaArtiklu]
	@IdArtikl int
AS
	SELECT 
		CenaKus 
	FROM 
		dbo.Artikl 
	WHERE 
		idArtikl=@IdArtikl