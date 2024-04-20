namespace GameBase
{
    using System;
    using UnityEngine.SceneManagement;

    public static class Loader
    {
        public enum Scene
        {
            MainMenuScene,
            GameScene,
            LoadingScene
        }
        private static Scene targetScene;

        public static void Load(Scene targetSceneParam)
        {
            Loader.targetScene = targetSceneParam;
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(targetScene.ToString());
        }
    }
}
