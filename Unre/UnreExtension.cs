using System;
using System.Collections.Generic;
using System.Text;

namespace Unre
{
    public static class UnreExtension
    {
        public static T Do<T>(this T entity) where T : class
        {
            var repo = UnreRepository<T>.Instance;
            repo.Do(entity);
            return entity;
        }

        public static T Undo<T>(this T entity, bool isKeepReturnWhenNotUndo = false) where T : class
        {
            var repo = UnreRepository<T>.Instance;
            if (repo.IsCanUndo) return repo.Undo(entity);
            if (isKeepReturnWhenNotUndo) return entity;
            return null;
        }

        public static T Redo<T>(this T entity, bool isKeepReturnWhenNotUndo = false) where T : class
        {
            var repo = UnreRepository<T>.Instance;
            if (repo.IsCanRedo) return repo.Redo(entity);
            if (isKeepReturnWhenNotUndo) return entity;
            return null;
        }

        public static bool IsStateEmpty<T>(this T entity) where T : class => UnreRepository<T>.Instance.IsStateEmpty;

        public static bool IsCanUndo<T>(this T entity) where T : class => UnreRepository<T>.Instance.IsCanUndo;

        public static bool IsCanRedo<T>(this T entity) where T : class => UnreRepository<T>.Instance.IsCanRedo;
    }
}
