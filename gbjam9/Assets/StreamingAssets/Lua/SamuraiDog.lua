function onUpdate(entity) 
    p = entity.position
    -- p.x = p.x + (entity.movement.speed * deltaTime)
    -- p.y = p.y + (entity.movement.speed * deltaTime)
    -- entity.position = p
    
    -- chaseTargets = entity.abilities.chase.targets
    
    chaseTargets = entity.abilities["Chase"].targets
    
    if (entity.states["Chase"])
    then
        -- entity.control.direction = chaseTargets[0].position - entity.position
        return
    end
    
    if (#chaseTargets > 0)
    then
        entity.states.Enter("ChasingPlayer")
        return
    end
    
    if (entity.states["Test"])
    then
        p.x = p.x + (entity.movement.speed * deltaTime)
        entity.states.Exit("Test")
        -- entity = entity - "Test"
    else
        p.y = p.y + (entity.movement.speed * deltaTime)
        --entity = entity + "Test"
        entity.states.Enter("Test")
    end
    
    entity.position = p
end