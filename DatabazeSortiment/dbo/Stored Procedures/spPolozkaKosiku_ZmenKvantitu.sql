CREATE PROCEDURE [dbo].[spPolozkaKosiku_ZmenKvantitu]
	@PocetKusu int = 0,
	@IdUzivatel nvarchar(450),
	@IdArtikl int
AS
        UPDATE [dbo].[PolozkaKosiku] 
		SET PocetKusu = @PocetKusu  
		WHERE (IdUzivatel =@IdUzivatel) 
					AND 
			  (IdArtikl = @IdArtikl);

SELECT @@ROWCOUNT;