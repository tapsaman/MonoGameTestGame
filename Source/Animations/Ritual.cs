using Microsoft.Xna.Framework;

namespace ZA6.Animations
{
    public class Ritual : Animation
    {
        public Ritual(DeadLink link, DeadLinkHat hat)
        {
            link.SpriteOffset.Y = -60f;

            Stages = new AnimationStage[]
            {
                new WaitStage(9f),
                new HoverStage(link, 35f, 6f),
                new HoverStage(link, -5f, 2f),
                new HoverStage(link, 5f, 2f),
                new HoverStage(link, -5f, 2f),
                new HoverStage(link, 5f, 2f),
                new HoverStage(link, -5f, 2f),
                new HoverStage(link, 5f, 2f),
                new RitualFinishStage(link, 5f, 2f, hat),
                //new Move.MoveStage(hat, new Vector2(0, 96f), 1f),
            };
        }

        private class HoverStage : AnimationStage
        {
            private MapObject _target;
            private Vector2 _defaultOffset;
            private float _distance;
            private float _time;

            public HoverStage(MapObject target, float distance, float time)
            {
                _target = target;
                _distance = distance;
                _time = time;
            }

            public override void Update(float elapsedTime)
            {
                if (elapsedTime < _time)
                {
                    _target.SpriteOffset.Y += _distance * (Animation.Delta / _time);
                }
                else
                {
                    IsDone = true;
                }
            }
        }

        private class RitualFinishStage : AnimationStage
        {
            private MapObject _target;
            private DeadLinkHat _hat;
            private Vector2 _hatStartPosition;
            private float _distance;
            private float _hatDistance = 96f;
            private float _time = 5f;

            public RitualFinishStage(MapObject target, float distance, float time, DeadLinkHat hat)
            {
                _target = target;
                _distance = distance;
                _time = time;
                _hat = hat;
                _hatStartPosition = _hat.Position;
            }

            public override void Update(float elapsedTime)
            {
                _target.SpriteOffset.Y += _distance * (Animation.Delta / _time);


                if (elapsedTime < _time / 4)
                {
                    _hat.Position = _hatStartPosition + new Vector2(0, _hatDistance * (elapsedTime / (_time / 4)));
                }
                else if (elapsedTime < _time / 2)
                {
                    _hat.AnimatedSprite.SetAnimation("Dropped");
                }
                else
                {
                    IsDone = true;
                }
            }
        }
    }
}