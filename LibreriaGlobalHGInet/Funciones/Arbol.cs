using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Funciones
{
    /// <summary>
    /// Crea un objeto de tipo treelist(Nodo) que sirve para organizar estructuras de objetos o listas con un ID y un IDPadre
    /// Ejemplo:     
    /// Id: 1, IdPadre:0, Colombia
    /// Id: 2, IdPadre:1, Medellin
    /// Id: 3, IdPadre:2, Los Colores
    /// Id: 4, IdPadre:1, Bogota
    /// 
    /// Resultado:    
    ///     Id: 1, IdPadre:0, Colombia
    ///         Id: 2, IdPadre:1, Medellin
    ///             Id: 3, IdPadre:2, Los Colores
    ///     Id: 4, IdPadre:1, Bogota
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Arbol<T> : IEqualityComparer, IEnumerable<T>, IEnumerable<Arbol<T>>
    {
        public Arbol<T> Parent { get; private set; }
        public T Value { get; set; }
        private readonly List<Arbol<T>> _children = new List<Arbol<T>>();

        public Arbol(T value)
        {
            Value = value;
        }

        public Arbol<T> this[int index]
        {
            get
            {
                return _children[index];
            }
        }

        public Arbol<T> Add(T value, int index = -1)
        {
            var childArbol = new Arbol<T>(value);
            Add(childArbol, index);
            return childArbol;
        }

        public void Add(Arbol<T> childArbol, int index = -1)
        {
            if (index < -1)
            {
                throw new ArgumentException("The index can not be lower then -1");
            }
            if (index > Children.Count() - 1)
            {
                throw new ArgumentException("The index ({0}) can not be higher then index of the last iten. Use the AddChild() method without an index to add at the end".FormatInvariant(index));
            }
            if (!childArbol.IsRoot)
            {
                throw new ArgumentException("The child Arbol with value [{0}] can not be added because it is not a root Arbol.".FormatInvariant(childArbol.Value));
            }

            if (Root == childArbol)
            {
                throw new ArgumentException("The child Arbol with value [{0}] is the rootArbol of the parent.".FormatInvariant(childArbol.Value));
            }

            if (childArbol.SelfAndDescendants.Any(n => this == n))
            {
                throw new ArgumentException("The childArbol with value [{0}] can not be added to itself or its descendants.".FormatInvariant(childArbol.Value));
            }
            childArbol.Parent = this;
            if (index == -1)
            {
                _children.Add(childArbol);
            }
            else
            {
                _children.Insert(index, childArbol);
            }
        }

        public Arbol<T> AddFirstChild(T value)
        {
            var childArbol = new Arbol<T>(value);
            AddFirstChild(childArbol);
            return childArbol;
        }

        public void AddFirstChild(Arbol<T> childArbol)
        {
            Add(childArbol, 0);
        }

        public Arbol<T> AddFirstSibling(T value)
        {
            var childArbol = new Arbol<T>(value);
            AddFirstSibling(childArbol);
            return childArbol;
        }

        public void AddFirstSibling(Arbol<T> childArbol)
        {
            Parent.AddFirstChild(childArbol);
        }
        public Arbol<T> AddLastSibling(T value)
        {
            var childArbol = new Arbol<T>(value);
            AddLastSibling(childArbol);
            return childArbol;
        }

        public void AddLastSibling(Arbol<T> childArbol)
        {
            Parent.Add(childArbol);
        }

        public Arbol<T> AddParent(T value)
        {
            var newArbol = new Arbol<T>(value);
            AddParent(newArbol);
            return newArbol;
        }

        public void AddParent(Arbol<T> parentArbol)
        {
            if (!IsRoot)
            {
                throw new ArgumentException("This Arbol [{0}] already has a parent".FormatInvariant(Value), "parentArbol");
            }
            parentArbol.Add(this);
        }

        public IEnumerable<Arbol<T>> Ancestors
        {
            get
            {
                if (IsRoot)
                {
                    return Enumerable.Empty<Arbol<T>>();
                }
                return Parent.ToIEnumarable().Concat(Parent.Ancestors);
            }
        }

        public IEnumerable<Arbol<T>> Descendants
        {
            get
            {
                return SelfAndDescendants.Skip(1);
            }
        }

        public IEnumerable<Arbol<T>> Children
        {
            get
            {
                return _children;
            }
        }

        public IEnumerable<Arbol<T>> Siblings
        {
            get
            {
                return SelfAndSiblings.Where(Other);

            }
        }

        private bool Other(Arbol<T> Arbol)
        {
            return !ReferenceEquals(Arbol, this);
        }

        public IEnumerable<Arbol<T>> SelfAndChildren
        {
            get
            {
                return this.ToIEnumarable().Concat(Children);
            }
        }

        public IEnumerable<Arbol<T>> SelfAndAncestors
        {
            get
            {
                return this.ToIEnumarable().Concat(Ancestors);
            }
        }

        public IEnumerable<Arbol<T>> SelfAndDescendants
        {
            get
            {
                return this.ToIEnumarable().Concat(Children.SelectMany(c => c.SelfAndDescendants));
            }
        }

        public IEnumerable<Arbol<T>> SelfAndSiblings
        {
            get
            {
                if (IsRoot)
                {
                    return this.ToIEnumarable();
                }
                return Parent.Children;

            }
        }

        public IEnumerable<Arbol<T>> All
        {
            get
            {
                return Root.SelfAndDescendants;
            }
        }


        public IEnumerable<Arbol<T>> SameLevel
        {
            get
            {
                return SelfAndSameLevel.Where(Other);

            }
        }

        public int Level
        {
            get
            {
                return Ancestors.Count();
            }
        }

        public IEnumerable<Arbol<T>> SelfAndSameLevel
        {
            get
            {
                return GetArbolsAtLevel(Level);
            }
        }

        public IEnumerable<Arbol<T>> GetArbolsAtLevel(int level)
        {
            return Root.GetArbolsAtLevelInternal(level);
        }

        private IEnumerable<Arbol<T>> GetArbolsAtLevelInternal(int level)
        {
            if (level == Level)
            {
                return this.ToIEnumarable();
            }
            return Children.SelectMany(c => c.GetArbolsAtLevelInternal(level));
        }

        public Arbol<T> Root
        {
            get
            {
                return SelfAndAncestors.Last();
            }
        }

        public void Disconnect()
        {
            if (IsRoot)
            {
                throw new InvalidOperationException("The root Arbol [{0}] can not get disconnected from a parent.".FormatInvariant(Value));
            }
            Parent._children.Remove(this);
            Parent = null;
        }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _children.Values().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public IEnumerator<Arbol<T>> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static IEnumerable<Arbol<T>> CreateTree<TId>(IEnumerable<T> values, Func<T, TId> idSelector, Func<T, TId?> parentIdSelector)
            where TId : struct
        {
            var valuesCache = values.ToList();
            if (!valuesCache.Any())
                return Enumerable.Empty<Arbol<T>>();
            T itemWithIdAndParentIdIsTheSame = valuesCache.FirstOrDefault(v => IsSameId(idSelector(v), parentIdSelector(v)));
            if (itemWithIdAndParentIdIsTheSame != null) // Hier verwacht je ook een null terug te kunnen komen
            {
                throw new ArgumentException("At least one value has the samen Id and parentId [{0}]".FormatInvariant(itemWithIdAndParentIdIsTheSame));
            }

            var Arbols = valuesCache.Select(v => new Arbol<T>(v));
            return CreateTree(Arbols, idSelector, parentIdSelector);

        }

        public static IEnumerable<Arbol<T>> CreateTree<TId>(IEnumerable<Arbol<T>> rootArbols, Func<T, TId> idSelector, Func<T, TId?> parentIdSelector)
            where TId : struct

        {
            var rootArbolsCache = rootArbols.ToList();
            var duplicates = rootArbolsCache.Duplicates(n => n).ToList();
            if (duplicates.Any())
            {
                throw new ArgumentException("One or more values contains {0} duplicate keys. The first duplicate is: [{1}]".FormatInvariant(duplicates.Count, duplicates[0]));
            }

            foreach (var rootArbol in rootArbolsCache)
            {
                var parentId = parentIdSelector(rootArbol.Value);
                var parent = rootArbolsCache.FirstOrDefault(n => IsSameId(idSelector(n.Value), parentId));

                if (parent != null)
                {
                    parent.Add(rootArbol);
                }
                else if (parentId != null)
                {
                    // throw new ArgumentException("A value has the parent ID [{0}] but no other Arbols has this ID".FormatInvariant(parentId.Value));
                }
            }
            var result = rootArbolsCache.Where(n => n.IsRoot);
            return result;
        }


        private static bool IsSameId<TId>(TId id, TId? parentId)
            where TId : struct
        {
            return parentId != null && id.Equals(parentId.Value);
        }

        #region Equals en ==

        public static bool operator ==(Arbol<T> value1, Arbol<T> value2)
        {
            if ((object)(value1) == null && (object)value2 == null)
            {
                return true;
            }
            return ReferenceEquals(value1, value2);
        }

        public static bool operator !=(Arbol<T> value1, Arbol<T> value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(Object anderePeriode)
        {
            var valueThisType = anderePeriode as Arbol<T>;
            return this == valueThisType;
        }

        public bool Equals(Arbol<T> value)
        {
            return this == value;
        }

        public bool Equals(Arbol<T> value1, Arbol<T> value2)
        {
            return value1 == value2;
        }

        bool IEqualityComparer.Equals(object value1, object value2)
        {
            var valueThisType1 = value1 as Arbol<T>;
            var valueThisType2 = value2 as Arbol<T>;

            return Equals(valueThisType1, valueThisType2);
        }

        public int GetHashCode(object obj)
        {
            return GetHashCode(obj as Arbol<T>);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public int GetHashCode(Arbol<T> value)
        {
            return base.GetHashCode();
        }
        #endregion
    }







    public static class ArbolExtensions
    {
        public static IEnumerable<T> Values<T>(this IEnumerable<Arbol<T>> Arbols)
        {
            return Arbols.Select(n => n.Value);
        }
    }

    public static class OtherExtensions
    {
        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            var grouped = source.GroupBy(selector);
            var moreThen1 = grouped.Where(i => i.IsMultiple());

            return moreThen1.SelectMany(i => i);
        }

        public static bool IsMultiple<T>(this IEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            return enumerator.MoveNext() && enumerator.MoveNext();
        }

        public static IEnumerable<T> ToIEnumarable<T>(this T item)
        {
            yield return item;
        }

        public static string FormatInvariant(this string text, params object[] parameters)
        {
            // This is not the "real" implementation, but that would go out of Scope
            return string.Format(CultureInfo.InvariantCulture, text, parameters);
        }
    }

}
