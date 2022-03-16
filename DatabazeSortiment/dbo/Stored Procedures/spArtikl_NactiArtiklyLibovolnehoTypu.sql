CREATE PROCEDURE [dbo].[spArtikl_NactiArtiklyLibovolnehoTypu]
	@Offset int,
	@Pocet int
AS
	SELECT 
		* 
	FROM 
		dbo.Artikl 
	ORDER BY 
		CenaKus 
	OFFSET @Offset ROWS FETCH NEXT @Pocet ROWS ONLY