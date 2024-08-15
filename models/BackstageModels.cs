namespace models;

public class BackstageYaml
{
    public string apiVersion {get;set; } = "backstage.io/v1alpha1";
    public string kind { get;set; } = "User";
    public Metadata metadata { get; set; } = new();
    public Spec spec { get; set; } = new();
}

public class Metadata
{
    public string name { get;set; } = null!;
}

public class Spec
{
    public Profile profile { get; set; } = new();

    [SharpYaml.Serialization.YamlStyle(SharpYaml.YamlStyle.Flow)]
    public List<string> memberOf { get; set; } = new(); // can be empty list but never null
}

public class Profile
{
    public string displayName { get; set; } = null!;
    public string email { get; set; } = null!;
}

