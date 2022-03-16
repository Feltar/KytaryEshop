CREATE PROCEDURE [dbo].[spArtikl_NactiArtiklyUrcitehoTypu]
	@Typ int,
	@Offset int,
	@Pocet int
AS
	SELECT 
		* 
	FROM 
		dbo.Artikl 
	WHERE 
		(TypArtiklu = @Typ) 
	ORDER BY 
		CenaKus 
	OFFSET @Offset ROWS FETCH NEXT @Pocet ROWS ONLY