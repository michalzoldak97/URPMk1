using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class SSMStartEndEvent
    {
        private SceneStartManager startManager;
        private SceneIndex currScene;
        public SSMStartEndEvent(SceneStartManager startManager, int currSceneIdx)
        {
            this.startManager = startManager;
            currScene = (SceneIndex)currSceneIdx;
        }
        public void OnStart()
        {
            switch (currScene)
            {
                case (SceneIndex.PLAN):
                    {
                        startManager.CallEventStartPlan();
                        break;
                    }
                case (SceneIndex.GAME_BEST):
                    {
                        startManager.CallEventStartGame();
                        break;
                    }
                case (SceneIndex.GAME_GOOD):
                    {
                        startManager.CallEventStartGame();
                        break;
                    }
                case (SceneIndex.GAME_LOW):
                    {
                        startManager.CallEventStartGame();
                        break;
                    }
            }
        }
        public void OnEnd()
        {
            switch (currScene)
            {
                case (SceneIndex.PLAN):
                    {
                        startManager.CallEventEndPlan();
                        break;
                    }
                case (SceneIndex.GAME_BEST):
                    {
                        startManager.CallEventEndGameScene();
                        break;
                    }
                case (SceneIndex.GAME_GOOD):
                    {
                        startManager.CallEventEndGameScene();
                        break;
                    }
                case (SceneIndex.GAME_LOW):
                    {
                        startManager.CallEventEndGameScene();
                        break;
                    }
            }
        }
    }
}
