CREATE PROCEDURE [dbo].[spArtikl_PocetArtikluPresVsechnyTypy]
AS
	SELECT 
		COUNT(*) 
	FROM 
		dbo.Artikl