function onUpdate(entity) 
    p = entity.position
    -- p.x = p.x + (entity.movement.speed * deltaTime)
    p.y = p.y + (entity.movement.speed * deltaTime)
    entity.position = p
end