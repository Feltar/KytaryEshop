CREATE PROCEDURE [dbo].[spArtikl_NactiArtikl]
	@IdArtikl int
AS
	SELECT 
		* 
	FROM 
		dbo.Artikl 
	WHERE 
		IdArtikl = @IdArtikl
RETURN 0