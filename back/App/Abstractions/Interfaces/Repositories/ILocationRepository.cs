namespace Transport.Api.Abstractions.Interfaces.Repositories;

public enum Region
{
    AuvergneRhoneAlpes,
    BourgogneFrancheComte,
    Bretagne,
    CentreValDeLoire,
    Corse,
    GrandEst,
    HautDeFrance,
    Normandie,
    NouvelleAquitaine,
    IleDeFrance,
    Occitanie,
    PaysDeLaLoire,
    ProvenceAlpesCoteAzur
}

public interface ILocationRepository
{
    /// <returns>
    ///     All postal codes with at least one fuel station
    /// </returns>
    Task<List<string>> GetPostalCodes();

    /// <returns>
    ///     All postal codes with at least one fuel station for a given region
    /// </returns>
    Task<List<string>> GetPostalCodes(Region region);
}