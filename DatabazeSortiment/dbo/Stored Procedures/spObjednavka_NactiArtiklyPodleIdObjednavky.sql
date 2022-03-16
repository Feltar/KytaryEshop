CREATE PROCEDURE [dbo].[spObjednavka_NactiArtiklyPodleIdObjednavky]
	@IdObjednavka int
AS
	SELECT 
		ar.IdArtikl, ar.CenaKus, ar.Znacka, ar.NazevProdukt, ar.KusuNaSklade, ar.TypArtiklu
	FROM 
		[dbo].[Artikl] as ar JOIN [dbo].[ObjednavkaArtikl] as oa 
	ON 
		(ar.IdArtikl = oa.IdArtikl) 
    WHERE 
		oa.IdObjednavka = @IdObjednavka