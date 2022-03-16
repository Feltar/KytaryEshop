CREATE PROCEDURE [dbo].[spPolozkaKosiku_UlozPolozkuUzivatele]
	@IdUzivatel nvarchar(450),
	@IdArtikl int,
	@PocetKusu int
AS
	 INSERT INTO [dbo].[PolozkaKosiku] 
				(IdUzivatel, IdArtikl, PocetKusu) 
	 VALUES 
				(@IdUzivatel, @IdArtikl, @PocetKusu);
	SELECT @@ROWCOUNT;