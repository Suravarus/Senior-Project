using System;
using UnityEngine;

using Utilities;
namespace Combat.Animation
{
    /// <summary>
    /// Handles the transition of animations based on Combatant 
    /// behaviour.
    /// <list type="bullet">
    /// <item>For now, all characters will look in the direction that their weapons are pointing.</item>
    /// </list>
    /// </summary>
    public class PuppetMaster
    {
        /// <summary>
        /// The Combatant that will be animated by this PuppetMaster.
        /// </summary>
        public WeaponWielder Puppet { get; set; }
        Boolean PuppetIsPlayer 
        { get => this.Puppet.GetComponent<PlayerMovement>() != null; }

        private Animator CharacterAnimator { get; set; }
        public PuppetMaster(Animator animator, WeaponWielder puppet)
        {
            this.CharacterAnimator = animator;
            this.Puppet = puppet;
        }

        /// <summary>
        /// Assigns animations to the puppet. 
        /// This method should be called in Combatant.Update().
        /// </summary>
        public void PullTheStrings()
        {
            if (this.Puppet.IsAlive())
            {
                // DETERMINE if player is moving.
                Boolean puppetIsMoving = this.Puppet.GetComponent<Rigidbody2D>().velocity.magnitude > 0;
                // CALCULATE the vector from the puppet's weapon to it's chest
                var target = (!this.Puppet.Disarmed()
                    ? this.Puppet.RangedWeapon.GetGameObject().transform.position
                    : Camera.main.ScreenToWorldPoint(this.Puppet.GetComponent<PlayerMovement>().GetCursorPosition()));
                var v = target - this.Puppet.GetBodyTransform(Combatant.BodyPart.Chest).position;
                v = v.normalized;
                // CALCULATE the direction the weapon is facing
                var direction = PhysicsTool.DirectionFromHorizontal(v);
                // IF player is moving
                if (puppetIsMoving)
                {
                    // SET run animation based on direction
                    switch (direction)
                    {
                        case PhysicsTool.Direction.Down:
                            this.CharacterAnimator.Play(RunningStateName.RunDown.ToString());
                            break;
                        case PhysicsTool.Direction.Up:
                            this.CharacterAnimator.Play(RunningStateName.RunUp.ToString());
                            break;
                        case PhysicsTool.Direction.Left:
                            this.CharacterAnimator.Play(RunningStateName.RunLeft.ToString());
                            break;
                        case PhysicsTool.Direction.Right:
                            this.CharacterAnimator.Play(RunningStateName.RunRight.ToString());
                            break;
                    }
                }
                else // ELSE
                {
                    // SET idle animation based on direction
                    switch (direction)
                    {
                        case PhysicsTool.Direction.Down:
                            this.CharacterAnimator.Play(IdleStateName.IdleDown.ToString());
                            break;
                        case PhysicsTool.Direction.Left:
                            this.CharacterAnimator.Play(IdleStateName.IdleLeft.ToString());
                            break;
                        case PhysicsTool.Direction.Up:
                            this.CharacterAnimator.Play(IdleStateName.IdleUp.ToString());
                            break;
                        case PhysicsTool.Direction.Right:
                            this.CharacterAnimator.Play(IdleStateName.IdleRight.ToString());
                            break;
                    }
                }
            }
            else
            {
                this.CharacterAnimator.StopPlayback();
                this.CharacterAnimator.enabled = false;
            }
        }
    }
}
