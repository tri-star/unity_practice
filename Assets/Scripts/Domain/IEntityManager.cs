#nullable enable
using UnityEngine;

namespace ActionSample.Domain
{
    public interface IEntityManager
    {
        GameObject? FindObjectWithTag(string tag, bool useCache = false);


    }
}
