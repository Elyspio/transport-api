
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;


namespace Adapters.FuelStation;

public partial class FuelStations
{
    [JsonProperty("?xml", Required = Required.Always)]
    public Xml Xml { get; set; }

    [JsonProperty("pdv_liste", Required = Required.Always)]
    public PdvListe PdvListe { get; set; }
}

public partial class PdvListe
{
    [JsonProperty("pdv", Required = Required.Always)]
    public List<Pdv> Pdv { get; set; }
}

public partial class Pdv
{
    [JsonProperty("@id", Required = Required.Always)]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Id { get; set; }

    [JsonProperty("@latitude", Required = Required.Always)]
    public string Latitude { get; set; }

    [JsonProperty("@longitude", Required = Required.Always)]
    public string Longitude { get; set; }

    [JsonProperty("@cp", Required = Required.Always)]
    public string Cp { get; set; }

    [JsonProperty("@pop", Required = Required.Always)]
    public Pop Pop { get; set; }

    [JsonProperty("adresse", Required = Required.Always)]
    public string Adresse { get; set; }

    [JsonProperty("ville")]
    public string Ville { get; set; }

    [JsonProperty("horaires", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public Horaires Horaires { get; set; }

    [JsonProperty("services", Required = Required.AllowNull)]
    public Services Services { get; set; }

    [JsonProperty("prix", NullValueHandling = NullValueHandling.Ignore)]
    public PrixUnion? Prix { get; set; }
}

public partial class Horaires
{
    [JsonProperty("@automate-24-24", Required = Required.Always)]
    public string Automate2424 { get; set; }

    [JsonProperty("jour", Required = Required.Always)]
    public List<Jour> Jour { get; set; }
}

public partial class Jour
{
    [JsonProperty("@id", Required = Required.Always)]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Id { get; set; }

    [JsonProperty("@nom", Required = Required.Always)]
    public JourNom Nom { get; set; }

    [JsonProperty("@ferme", Required = Required.Always)]
    public string Ferme { get; set; }

    [JsonProperty("horaire", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
    public HoraireUnion? Horaire { get; set; }
}

public partial class HoraireElement
{
    [JsonProperty("@ouverture", Required = Required.Always)]
    public string Ouverture { get; set; }

    [JsonProperty("@fermeture", Required = Required.Always)]
    public string Fermeture { get; set; }
}

public partial class PrixElement
{
    [JsonProperty("@nom", Required = Required.Always)]
    public FuelType Nom { get; set; }

    [JsonProperty("@id", Required = Required.Always)]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Id { get; set; }

    [JsonProperty("@maj", Required = Required.Always)]
    public DateTimeOffset Maj { get; set; }

    [JsonProperty("@valeur", Required = Required.Always)]
    public string Valeur { get; set; }
}

public partial class Services
{
    [JsonProperty("service", Required = Required.Always)]
    public ServiceUnion Service { get; set; }
}

public partial class Xml
{
    [JsonProperty("@version", Required = Required.Always)]
    public string Version { get; set; }

    [JsonProperty("@encoding", Required = Required.Always)]
    public string Encoding { get; set; }

    [JsonProperty("@standalone", Required = Required.Always)]
    public string Standalone { get; set; }
}

public enum JourNom { Dimanche, Jeudi, Lundi, Mardi, Mercredi, Samedi, Vendredi };

public enum Pop { A, N, R };

public enum FuelType { E10, E85, Gazole, GpLc, Sp95, Sp98 };

public enum ServiceElement { AireDeCampingCars, AutomateCb2424, Bar, BornesÉlectriques, BoutiqueAlimentaire, BoutiqueNonAlimentaire, CarburantAdditivé, DabDistributeurAutomatiqueDeBillets, Douches, EspaceBébé, Gnv, LavageAutomatique, LavageManuel, Laverie, LocationDeVéhicule, PistePoidsLourds, RelaisColis, RestaurationSurPlace, RestaurationÀEmporter, ServicesRéparationEntretien, StationDeGonflage, ToilettesPubliques, VenteDAdditifsCarburants, VenteDeFioulDomestique, VenteDeGazDomestiqueButanePropane, VenteDePétroleLampant, Wifi, Inconnu };

public partial struct HoraireUnion
{
    public HoraireElement HoraireElement;
    public List<HoraireElement> HoraireElementArray;

    public static implicit operator HoraireUnion(HoraireElement HoraireElement) => new HoraireUnion { HoraireElement = HoraireElement };
    public static implicit operator HoraireUnion(List<HoraireElement> HoraireElementArray) => new HoraireUnion { HoraireElementArray = HoraireElementArray };
}

public partial struct PrixUnion
{
    public PrixElement PrixElement;
    public List<PrixElement> PrixElementArray;

    public static implicit operator PrixUnion(PrixElement PrixElement) => new PrixUnion { PrixElement = PrixElement };
    public static implicit operator PrixUnion(List<PrixElement> PrixElementArray) => new PrixUnion { PrixElementArray = PrixElementArray };
}

public partial struct ServiceUnion
{
    public ServiceElement? Enum;
    public List<ServiceElement> StringArray;

    public static implicit operator ServiceUnion(ServiceElement Enum) => new ServiceUnion { Enum = Enum };
    public static implicit operator ServiceUnion(List<ServiceElement> StringArray) => new ServiceUnion { StringArray = StringArray };
}

public partial class FuelStations
{
#pragma warning disable CS8603 // Possible null reference return.
    public static FuelStations FromJson(string json) => JsonConvert.DeserializeObject<FuelStations>(json, Converter.Settings);
#pragma warning restore CS8603 // Possible null reference return.
}

public static class Serialize
{
    public static string ToJson(this FuelStations self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            PopConverter.Singleton,
            JourNomConverter.Singleton,
            HoraireUnionConverter.Singleton,
            PrixUnionConverter.Singleton,
            PrixNomConverter.Singleton,
            ServiceUnionConverter.Singleton,
            ServiceElementConverter.Singleton,
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}

internal class ParseStringConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        long l;
        if (Int64.TryParse(value, out l))
        {
            return l;
        }
        throw new Exception("Cannot unmarshal type long");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (long)untypedValue;
        serializer.Serialize(writer, value.ToString());
        return;
    }

    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
}

internal class PopConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(Pop) || t == typeof(Pop?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "A":
                return Pop.A;
            case "N":
                return Pop.N;
            case "R":
                return Pop.R;
        }
        return null;
        throw new Exception("Cannot unmarshal type Pop");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (Pop)untypedValue;
        switch (value)
        {
            case Pop.A:
                serializer.Serialize(writer, "A");
                return;
            case Pop.N:
                serializer.Serialize(writer, "N");
                return;
            case Pop.R:
                serializer.Serialize(writer, "R");
                return;
        }
        throw new Exception("Cannot marshal type Pop");
    }

    public static readonly PopConverter Singleton = new PopConverter();
}

internal class JourNomConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(JourNom) || t == typeof(JourNom?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "Dimanche":
                return JourNom.Dimanche;
            case "Jeudi":
                return JourNom.Jeudi;
            case "Lundi":
                return JourNom.Lundi;
            case "Mardi":
                return JourNom.Mardi;
            case "Mercredi":
                return JourNom.Mercredi;
            case "Samedi":
                return JourNom.Samedi;
            case "Vendredi":
                return JourNom.Vendredi;
        }
        throw new Exception("Cannot unmarshal type JourNom");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (JourNom)untypedValue;
        switch (value)
        {
            case JourNom.Dimanche:
                serializer.Serialize(writer, "Dimanche");
                return;
            case JourNom.Jeudi:
                serializer.Serialize(writer, "Jeudi");
                return;
            case JourNom.Lundi:
                serializer.Serialize(writer, "Lundi");
                return;
            case JourNom.Mardi:
                serializer.Serialize(writer, "Mardi");
                return;
            case JourNom.Mercredi:
                serializer.Serialize(writer, "Mercredi");
                return;
            case JourNom.Samedi:
                serializer.Serialize(writer, "Samedi");
                return;
            case JourNom.Vendredi:
                serializer.Serialize(writer, "Vendredi");
                return;
        }
        throw new Exception("Cannot marshal type JourNom");
    }

    public static readonly JourNomConverter Singleton = new JourNomConverter();
}

internal class HoraireUnionConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(HoraireUnion) || t == typeof(HoraireUnion?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.StartObject:
                var objectValue = serializer.Deserialize<HoraireElement>(reader);
#pragma warning disable CS8601 // Possible null reference assignment.
                return new HoraireUnion { HoraireElement = objectValue };
#pragma warning restore CS8601 // Possible null reference assignment.
            case JsonToken.StartArray:
                var arrayValue = serializer.Deserialize<List<HoraireElement>>(reader);
#pragma warning disable CS8601 // Possible null reference assignment.
                return new HoraireUnion { HoraireElementArray = arrayValue };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        throw new Exception("Cannot unmarshal type HoraireUnion");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
#pragma warning disable CS8605 // Unboxing a possibly null value.
        var value = (HoraireUnion)untypedValue;
#pragma warning restore CS8605 // Unboxing a possibly null value.
        if (value.HoraireElementArray != null)
        {
            serializer.Serialize(writer, value.HoraireElementArray);
            return;
        }
        if (value.HoraireElement != null)
        {
            serializer.Serialize(writer, value.HoraireElement);
            return;
        }
        throw new Exception("Cannot marshal type HoraireUnion");
    }

    public static readonly HoraireUnionConverter Singleton = new HoraireUnionConverter();
}

internal class PrixUnionConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(PrixUnion) || t == typeof(PrixUnion?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.StartObject:
                var objectValue = serializer.Deserialize<PrixElement>(reader);
#pragma warning disable CS8601 // Possible null reference assignment.
                return new PrixUnion { PrixElement = objectValue };
#pragma warning restore CS8601 // Possible null reference assignment.
            case JsonToken.StartArray:
                var arrayValue = serializer.Deserialize<List<PrixElement>>(reader);
#pragma warning disable CS8601 // Possible null reference assignment.
                return new PrixUnion { PrixElementArray = arrayValue };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        throw new Exception("Cannot unmarshal type PrixUnion");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
#pragma warning disable CS8605 // Unboxing a possibly null value.
        var value = (PrixUnion)untypedValue;
#pragma warning restore CS8605 // Unboxing a possibly null value.
        if (value.PrixElementArray != null)
        {
            serializer.Serialize(writer, value.PrixElementArray);
            return;
        }
        if (value.PrixElement != null)
        {
            serializer.Serialize(writer, value.PrixElement);
            return;
        }
        throw new Exception("Cannot marshal type PrixUnion");
    }

    public static readonly PrixUnionConverter Singleton = new PrixUnionConverter();
}

internal class PrixNomConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(FuelType) || t == typeof(FuelType?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "E10":
                return FuelType.E10;
            case "E85":
                return FuelType.E85;
            case "GPLc":
                return FuelType.GpLc;
            case "Gazole":
                return FuelType.Gazole;
            case "SP95":
                return FuelType.Sp95;
            case "SP98":
                return FuelType.Sp98;
        }
        throw new Exception("Cannot unmarshal type PrixNom");
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (FuelType)untypedValue;
        switch (value)
        {
            case FuelType.E10:
                serializer.Serialize(writer, "E10");
                return;
            case FuelType.E85:
                serializer.Serialize(writer, "E85");
                return;
            case FuelType.GpLc:
                serializer.Serialize(writer, "GPLc");
                return;
            case FuelType.Gazole:
                serializer.Serialize(writer, "Gazole");
                return;
            case FuelType.Sp95:
                serializer.Serialize(writer, "SP95");
                return;
            case FuelType.Sp98:
                serializer.Serialize(writer, "SP98");
                return;
        }
        throw new Exception("Cannot marshal type PrixNom");
    }

    public static readonly PrixNomConverter Singleton = new PrixNomConverter();
}

internal class ServiceUnionConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(ServiceUnion) || t == typeof(ServiceUnion?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.String:
            case JsonToken.Date:
                var stringValue = serializer.Deserialize<string>(reader);
                switch (stringValue)
                {
                    case "Aire de camping-cars":
                        return new ServiceUnion { Enum = ServiceElement.AireDeCampingCars };
                    case "Automate CB 24/24":
                        return new ServiceUnion { Enum = ServiceElement.AutomateCb2424 };
                    case "Bar":
                        return new ServiceUnion { Enum = ServiceElement.Bar };
                    case "Bornes électriques":
                        return new ServiceUnion { Enum = ServiceElement.BornesÉlectriques };
                    case "Boutique alimentaire":
                        return new ServiceUnion { Enum = ServiceElement.BoutiqueAlimentaire };
                    case "Boutique non alimentaire":
                        return new ServiceUnion { Enum = ServiceElement.BoutiqueNonAlimentaire };
                    case "Carburant additivé":
                        return new ServiceUnion { Enum = ServiceElement.CarburantAdditivé };
                    case "DAB (Distributeur automatique de billets)":
                        return new ServiceUnion { Enum = ServiceElement.DabDistributeurAutomatiqueDeBillets };
                    case "Douches":
                        return new ServiceUnion { Enum = ServiceElement.Douches };
                    case "Espace bébé":
                        return new ServiceUnion { Enum = ServiceElement.EspaceBébé };
                    case "GNV":
                        return new ServiceUnion { Enum = ServiceElement.Gnv };
                    case "Lavage automatique":
                        return new ServiceUnion { Enum = ServiceElement.LavageAutomatique };
                    case "Lavage manuel":
                        return new ServiceUnion { Enum = ServiceElement.LavageManuel };
                    case "Laverie":
                        return new ServiceUnion { Enum = ServiceElement.Laverie };
                    case "Location de véhicule":
                        return new ServiceUnion { Enum = ServiceElement.LocationDeVéhicule };
                    case "Piste poids lourds":
                        return new ServiceUnion { Enum = ServiceElement.PistePoidsLourds };
                    case "Relais colis":
                        return new ServiceUnion { Enum = ServiceElement.RelaisColis };
                    case "Restauration sur place":
                        return new ServiceUnion { Enum = ServiceElement.RestaurationSurPlace };
                    case "Restauration à emporter":
                        return new ServiceUnion { Enum = ServiceElement.RestaurationÀEmporter };
                    case "Services réparation / entretien":
                        return new ServiceUnion { Enum = ServiceElement.ServicesRéparationEntretien };
                    case "Station de gonflage":
                        return new ServiceUnion { Enum = ServiceElement.StationDeGonflage };
                    case "Toilettes publiques":
                        return new ServiceUnion { Enum = ServiceElement.ToilettesPubliques };
                    case "Vente d'additifs carburants":
                        return new ServiceUnion { Enum = ServiceElement.VenteDAdditifsCarburants };
                    case "Vente de fioul domestique":
                        return new ServiceUnion { Enum = ServiceElement.VenteDeFioulDomestique };
                    case "Vente de gaz domestique (Butane, Propane)":
                        return new ServiceUnion { Enum = ServiceElement.VenteDeGazDomestiqueButanePropane };
                    case "Vente de pétrole lampant":
                        return new ServiceUnion { Enum = ServiceElement.VenteDePétroleLampant };
                    case "Wifi":
                        return new ServiceUnion { Enum = ServiceElement.Wifi };
                }
                break;
            case JsonToken.StartArray:
                var arrayValue = serializer.Deserialize<List<ServiceElement>>(reader);
#pragma warning disable CS8601 // Possible null reference assignment.
                return new ServiceUnion { StringArray = arrayValue };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        return new ServiceUnion { Enum = ServiceElement.Inconnu };
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
#pragma warning disable CS8605 // Unboxing a possibly null value.
        var value = (ServiceUnion)untypedValue;
#pragma warning restore CS8605 // Unboxing a possibly null value.
        if (value.Enum != null)
        {
            switch (value.Enum)
            {
                case ServiceElement.AireDeCampingCars:
                    serializer.Serialize(writer, "Aire de camping-cars");
                    return;
                case ServiceElement.AutomateCb2424:
                    serializer.Serialize(writer, "Automate CB 24/24");
                    return;
                case ServiceElement.Bar:
                    serializer.Serialize(writer, "Bar");
                    return;
                case ServiceElement.BornesÉlectriques:
                    serializer.Serialize(writer, "Bornes électriques");
                    return;
                case ServiceElement.BoutiqueAlimentaire:
                    serializer.Serialize(writer, "Boutique alimentaire");
                    return;
                case ServiceElement.BoutiqueNonAlimentaire:
                    serializer.Serialize(writer, "Boutique non alimentaire");
                    return;
                case ServiceElement.CarburantAdditivé:
                    serializer.Serialize(writer, "Carburant additivé");
                    return;
                case ServiceElement.DabDistributeurAutomatiqueDeBillets:
                    serializer.Serialize(writer, "DAB (Distributeur automatique de billets)");
                    return;
                case ServiceElement.Douches:
                    serializer.Serialize(writer, "Douches");
                    return;
                case ServiceElement.EspaceBébé:
                    serializer.Serialize(writer, "Espace bébé");
                    return;
                case ServiceElement.Gnv:
                    serializer.Serialize(writer, "GNV");
                    return;
                case ServiceElement.LavageAutomatique:
                    serializer.Serialize(writer, "Lavage automatique");
                    return;
                case ServiceElement.LavageManuel:
                    serializer.Serialize(writer, "Lavage manuel");
                    return;
                case ServiceElement.Laverie:
                    serializer.Serialize(writer, "Laverie");
                    return;
                case ServiceElement.LocationDeVéhicule:
                    serializer.Serialize(writer, "Location de véhicule");
                    return;
                case ServiceElement.PistePoidsLourds:
                    serializer.Serialize(writer, "Piste poids lourds");
                    return;
                case ServiceElement.RelaisColis:
                    serializer.Serialize(writer, "Relais colis");
                    return;
                case ServiceElement.RestaurationSurPlace:
                    serializer.Serialize(writer, "Restauration sur place");
                    return;
                case ServiceElement.RestaurationÀEmporter:
                    serializer.Serialize(writer, "Restauration à emporter");
                    return;
                case ServiceElement.ServicesRéparationEntretien:
                    serializer.Serialize(writer, "Services réparation / entretien");
                    return;
                case ServiceElement.StationDeGonflage:
                    serializer.Serialize(writer, "Station de gonflage");
                    return;
                case ServiceElement.ToilettesPubliques:
                    serializer.Serialize(writer, "Toilettes publiques");
                    return;
                case ServiceElement.VenteDAdditifsCarburants:
                    serializer.Serialize(writer, "Vente d'additifs carburants");
                    return;
                case ServiceElement.VenteDeFioulDomestique:
                    serializer.Serialize(writer, "Vente de fioul domestique");
                    return;
                case ServiceElement.VenteDeGazDomestiqueButanePropane:
                    serializer.Serialize(writer, "Vente de gaz domestique (Butane, Propane)");
                    return;
                case ServiceElement.VenteDePétroleLampant:
                    serializer.Serialize(writer, "Vente de pétrole lampant");
                    return;
                case ServiceElement.Wifi:
                    serializer.Serialize(writer, "Wifi");
                    return;
            }
        }
        if (value.StringArray != null)
        {
            serializer.Serialize(writer, value.StringArray);
            return;
        }
        throw new Exception("Cannot marshal type ServiceUnion");
    }

    public static readonly ServiceUnionConverter Singleton = new ServiceUnionConverter();
}

internal class ServiceElementConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(ServiceElement) || t == typeof(ServiceElement?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "Aire de camping-cars":
                return ServiceElement.AireDeCampingCars;
            case "Automate CB 24/24":
                return ServiceElement.AutomateCb2424;
            case "Bar":
                return ServiceElement.Bar;
            case "Bornes électriques":
                return ServiceElement.BornesÉlectriques;
            case "Boutique alimentaire":
                return ServiceElement.BoutiqueAlimentaire;
            case "Boutique non alimentaire":
                return ServiceElement.BoutiqueNonAlimentaire;
            case "Carburant additivé":
                return ServiceElement.CarburantAdditivé;
            case "DAB (Distributeur automatique de billets)":
                return ServiceElement.DabDistributeurAutomatiqueDeBillets;
            case "Douches":
                return ServiceElement.Douches;
            case "Espace bébé":
                return ServiceElement.EspaceBébé;
            case "GNV":
                return ServiceElement.Gnv;
            case "Lavage automatique":
                return ServiceElement.LavageAutomatique;
            case "Lavage manuel":
                return ServiceElement.LavageManuel;
            case "Laverie":
                return ServiceElement.Laverie;
            case "Location de véhicule":
                return ServiceElement.LocationDeVéhicule;
            case "Piste poids lourds":
                return ServiceElement.PistePoidsLourds;
            case "Relais colis":
                return ServiceElement.RelaisColis;
            case "Restauration sur place":
                return ServiceElement.RestaurationSurPlace;
            case "Restauration à emporter":
                return ServiceElement.RestaurationÀEmporter;
            case "Services réparation / entretien":
                return ServiceElement.ServicesRéparationEntretien;
            case "Station de gonflage":
                return ServiceElement.StationDeGonflage;
            case "Toilettes publiques":
                return ServiceElement.ToilettesPubliques;
            case "Vente d'additifs carburants":
                return ServiceElement.VenteDAdditifsCarburants;
            case "Vente de fioul domestique":
                return ServiceElement.VenteDeFioulDomestique;
            case "Vente de gaz domestique (Butane, Propane)":
                return ServiceElement.VenteDeGazDomestiqueButanePropane;
            case "Vente de pétrole lampant":
                return ServiceElement.VenteDePétroleLampant;
            case "Wifi":
                return ServiceElement.Wifi;
        }
        return ServiceElement.Inconnu;
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (ServiceElement)untypedValue;
        switch (value)
        {
            case ServiceElement.AireDeCampingCars:
                serializer.Serialize(writer, "Aire de camping-cars");
                return;
            case ServiceElement.AutomateCb2424:
                serializer.Serialize(writer, "Automate CB 24/24");
                return;
            case ServiceElement.Bar:
                serializer.Serialize(writer, "Bar");
                return;
            case ServiceElement.BornesÉlectriques:
                serializer.Serialize(writer, "Bornes électriques");
                return;
            case ServiceElement.BoutiqueAlimentaire:
                serializer.Serialize(writer, "Boutique alimentaire");
                return;
            case ServiceElement.BoutiqueNonAlimentaire:
                serializer.Serialize(writer, "Boutique non alimentaire");
                return;
            case ServiceElement.CarburantAdditivé:
                serializer.Serialize(writer, "Carburant additivé");
                return;
            case ServiceElement.DabDistributeurAutomatiqueDeBillets:
                serializer.Serialize(writer, "DAB (Distributeur automatique de billets)");
                return;
            case ServiceElement.Douches:
                serializer.Serialize(writer, "Douches");
                return;
            case ServiceElement.EspaceBébé:
                serializer.Serialize(writer, "Espace bébé");
                return;
            case ServiceElement.Gnv:
                serializer.Serialize(writer, "GNV");
                return;
            case ServiceElement.LavageAutomatique:
                serializer.Serialize(writer, "Lavage automatique");
                return;
            case ServiceElement.LavageManuel:
                serializer.Serialize(writer, "Lavage manuel");
                return;
            case ServiceElement.Laverie:
                serializer.Serialize(writer, "Laverie");
                return;
            case ServiceElement.LocationDeVéhicule:
                serializer.Serialize(writer, "Location de véhicule");
                return;
            case ServiceElement.PistePoidsLourds:
                serializer.Serialize(writer, "Piste poids lourds");
                return;
            case ServiceElement.RelaisColis:
                serializer.Serialize(writer, "Relais colis");
                return;
            case ServiceElement.RestaurationSurPlace:
                serializer.Serialize(writer, "Restauration sur place");
                return;
            case ServiceElement.RestaurationÀEmporter:
                serializer.Serialize(writer, "Restauration à emporter");
                return;
            case ServiceElement.ServicesRéparationEntretien:
                serializer.Serialize(writer, "Services réparation / entretien");
                return;
            case ServiceElement.StationDeGonflage:
                serializer.Serialize(writer, "Station de gonflage");
                return;
            case ServiceElement.ToilettesPubliques:
                serializer.Serialize(writer, "Toilettes publiques");
                return;
            case ServiceElement.VenteDAdditifsCarburants:
                serializer.Serialize(writer, "Vente d'additifs carburants");
                return;
            case ServiceElement.VenteDeFioulDomestique:
                serializer.Serialize(writer, "Vente de fioul domestique");
                return;
            case ServiceElement.VenteDeGazDomestiqueButanePropane:
                serializer.Serialize(writer, "Vente de gaz domestique (Butane, Propane)");
                return;
            case ServiceElement.VenteDePétroleLampant:
                serializer.Serialize(writer, "Vente de pétrole lampant");
                return;
            case ServiceElement.Wifi:
                serializer.Serialize(writer, "Wifi");
                return;
        }
        throw new Exception("Cannot marshal type ServiceElement");
    }

    public static readonly ServiceElementConverter Singleton = new ServiceElementConverter();
}
