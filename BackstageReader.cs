using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.RepresentationModel;
using System.Text.Json;
using models;

namespace backstage_reader;

public class BackstageReader 
{

    public BackstageReader() 
    {
        // empty constructor
    }

    public List<BackstageYaml> GetListOfBackstageUserEntities() 
    {
        // list of backstage user entities
        var backstageUserEntities = new List<BackstageYaml>();

        // read yaml
        using (var reader = new StreamReader(@"catalogs/catalog.yaml")) 
        {

            var yaml = new YamlStream();
            yaml.Load(reader);
            var mappings = yaml.Documents;

            foreach (var mapping in mappings)
            {
                try
                {
                    var userEntity = (YamlMappingNode)(mapping.RootNode);
                    var entityProperties = userEntity.Children;

                    var newUserEntity = new BackstageYaml();

                    foreach (var entry in entityProperties)
                    {
                        var key = (YamlScalarNode)entry.Key;
                        var test3 = userEntity.Children[new YamlScalarNode(key.Value)];

                        if (key.Value.Equals("metadata"))
                        {
                            entityProperties.TryGetValue(key, out var metadataEntity);
                            newUserEntity.metadata.name = metadataEntity["name"].ToString();
                        }
                        if (key.Value.Equals("spec"))
                        {
                            entityProperties.TryGetValue(key, out var specProps);
                            var profileProp = specProps["profile"];

                            newUserEntity.spec.profile.displayName = profileProp["displayName"].ToString();
                            newUserEntity.spec.profile.email = profileProp["email"].ToString();

                            var memberOf = (specProps["memberOf"]).AllNodes.Where(x => x.AllNodes.Count() == 1).Select(x => x.ToString()).ToList();
                            newUserEntity.spec.memberOf = memberOf;

                        }

                    } // end of foreach properties
                    backstageUserEntities.Add(newUserEntity);

                } catch (Exception ex) {
                    continue; // if cannot cast to null object then there are no information to that node
                }
            }
            // return list of users
            return backstageUserEntities;
        }
    }

    public void WriteUserEntitiesToYaml(List<BackstageYaml> backstageUserEntities) 
    {
        foreach(var backstageUserEntity in backstageUserEntities) 
        {
            // write to yaml
            using (var writer = new StreamWriter(@"catalogs/catalog2.yaml", append: true))
            {
                writer.WriteLine("---");
                var serializer = new SharpYaml.Serialization.Serializer();
                serializer.Serialize(writer, backstageUserEntity);
                writer.Close();
            }
        }
    }    
}
    