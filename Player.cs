using Godot;
using System;
using GodotPlugins.Game;
public partial class Player : CharacterBody2D
{
    [Export]
    private float MaxSpeed = 100.0f;

    private float _currentSpeed = 0.0f;

    [Export]
    private float Acceleration = 10.5f;

    [Export]
    private float Decceleration = 7.5f;

    private Vector2 _currentVelocity; 

    public Vector2 CurrentVelocity 
    {
        get { return _currentVelocity; }
        private set {_currentVelocity = value; }

    }

    public bool IsReflecting {get; set;}

    [Export]
    public float ReflectTime = 0.5f;

    public Camera2D _followCamera;

    public Node2D _sprite;

    public CollisionShape2D _collisionBox;

    public Area2D _reflectBox;

    public override void _Ready()
    {
        _followCamera = GetNode<Camera2D>("FollowCamera");
        _sprite = GetNode("VisualSmooth").GetNode<Sprite2D>("Face");
        _collisionBox = GetNode<CollisionShape2D>("Collision");
        _reflectBox = GetNode<Area2D>("ReflectArea");
    }

    public override async void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("reflect")) 
        {
            IsReflecting = true;
            await(ToSignal(GetTree().CreateTimer(ReflectTime), "timeout"));
            IsReflecting = false;
        }

        if (IsReflecting) 
        {
            _sprite.Modulate = new Color(Colors.Red);
            _reflectBox.Modulate = new Color(Colors.MediumVioletRed);
            _collisionBox.Disabled = true;

        }
        else {
            _sprite.Modulate = new Color(Colors.SeaGreen);
            _reflectBox.Modulate = new Color(Colors.White);
            _collisionBox.Disabled = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Vector2 input = Input.GetVector("left", "right", "up", "down");
        input = input.Normalized();

        if (input != Vector2.Zero)
        {
            if (_currentSpeed < MaxSpeed) 
            {
                Accelerate((float)delta);
            }
        }
        else 
        {

                Deccelerate((float)delta);
        }

        _currentVelocity.X = Mathf.Round(input.X * _currentSpeed);
        _currentVelocity.Y = MathF.Round(input.Y * _currentSpeed);
        
        Velocity = _currentVelocity;
        MoveAndSlide();
    }

    private void Reflect() {

    }

    private void Accelerate(float dt) 
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, MaxSpeed, dt * Acceleration);

    }

    private void Deccelerate(float dt)
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, 0.0f, dt * Decceleration);
    }
}
