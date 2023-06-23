using Godot;
using System;

public partial class Projectile : CharacterBody2D
{
    Area2D _reflectHitbox;

    Sprite2D _sprite;
    bool _reflectCooldown;

    RandomNumberGenerator rng;

    int _splorgCountdown;
    public override void _Ready()
    {
        base._Ready();
        _reflectHitbox = GetNode("ProjectileSmoothing").GetNode<Area2D>("ReflectHitbox");
        _reflectHitbox.AreaEntered += area => GetReflected(area);
        _sprite = GetNode("ProjectileSmoothing").GetNode<Sprite2D>("ProjSprite");

        Velocity = Vector2.Down * 40.0f;
        rng = new RandomNumberGenerator();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _splorgCountdown++;
        if (_splorgCountdown >= 75 && !_reflectCooldown) 
        {
            _splorgCountdown = 0;
            Velocity += new Vector2(rng.RandfRange(-20.0f, 20.0f), rng.RandfRange(-20.0f, 20.0f));
        }

        MoveAndSlide();
    }
    private async void GetReflected(Area2D area) {
        if (area.IsInGroup("reflecting") && !_reflectCooldown)
        {
            this.Velocity = -Velocity;
            GD.Print("REFLECT");
            _reflectCooldown = true;
            _sprite.Modulate = new Color(Colors.Bisque);
        }
        
        await(ToSignal(GetTree().CreateTimer(1.0f), "timeout"));
        _reflectCooldown = false;
        _sprite.Modulate = new Color(Colors.White);
        }

}
