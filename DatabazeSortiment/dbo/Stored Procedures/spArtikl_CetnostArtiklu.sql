CREATE PROCEDURE [dbo].[spArtikl_CetnostArtiklu]
	@IdArtikl int
AS
	SELECT 
		KusuNaSklade 
	FROM 
		dbo.Artikl 
	WHERE 
		idArtikl=@IdArtikl