#if UNITY_EDITOR
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECS.Editor")]
[assembly: InternalsVisibleTo("ECS.Tests")]
#endif