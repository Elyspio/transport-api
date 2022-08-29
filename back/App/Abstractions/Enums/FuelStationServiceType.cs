using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Transport.Api.Abstractions.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum FuelStationServiceType
{
	AireDeCampingCars,
	AutomateCb2424,
	Bar,
	BornesÉlectriques,
	BoutiqueAlimentaire,
	BoutiqueNonAlimentaire,
	CarburantAdditivé,
	DabDistributeurAutomatiqueDeBillets,
	Douches,
	EspaceBébé,
	Gnv,
	LavageAutomatique,
	LavageManuel,
	Laverie,
	LocationDeVéhicule,
	PistePoidsLourds,
	RelaisColis,
	RestaurationSurPlace,
	RestaurationÀEmporter,
	ServicesRéparationEntretien,
	StationDeGonflage,
	ToilettesPubliques,
	VenteDAdditifsCarburants,
	VenteDeFioulDomestique,
	VenteDeGazDomestiqueButanePropane,
	VenteDePétroleLampant,
	Wifi
}