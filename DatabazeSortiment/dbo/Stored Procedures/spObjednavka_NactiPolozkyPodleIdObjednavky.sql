CREATE PROCEDURE [dbo].[spObjednavka_NactiPolozkyPodleIdObjednavky]
	@IdObjednavka int
AS
	SELECT 
		ar.IdArtikl as IdArtikl,  
		ar.Znacka as Znacka, 
		ar.NazevProdukt as NazevProdukt, 
		ar.TypArtiklu as TypArtiklu, 
		oa.CenaKus as CenaKus,
		oa.PocetKusu as PocetKusu
	FROM 
		[dbo].[Artikl] as ar JOIN [dbo].[ObjednavkaArtikl] as oa 
	ON 
		(ar.IdArtikl = oa.IdArtikl) 
    WHERE 
		oa.IdObjednavka = @IdObjednavka