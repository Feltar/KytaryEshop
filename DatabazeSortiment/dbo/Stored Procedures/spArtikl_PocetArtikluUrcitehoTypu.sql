CREATE PROCEDURE [dbo].[spArtikl_PocetArtikluUrcitehoTypu]
	@Typ int
AS
	SELECT 
		COUNT(*) 
	FROM 
		dbo.Artikl 
	WHERE
		TypArtiklu = @Typ;