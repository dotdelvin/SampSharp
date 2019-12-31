﻿// SampSharp
// Copyright 2019 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using SampSharp.Entities.Events;
using SampSharp.Entities.SAMP.Middleware;

namespace SampSharp.Entities.SAMP.Systems
{
    internal class ActorSystem : IConfiguringSystem
    {
        public void Configure(IEcsBuilder builder)
        {
            builder.EnableEvent<int, int>("OnActorStreamIn");
            builder.EnableEvent<int, int>("OnActorStreamOut");
            
            builder.UseMiddleware<EntityMiddleware>("OnActorStreamIn", 0, (Func<int, EntityId>) SampEntities.GetActorId, true);
            builder.UseMiddleware<EntityMiddleware>("OnActorStreamIn", 1, (Func<int, EntityId>) SampEntities.GetPlayerId);
            builder.UseMiddleware<EntityMiddleware>("OnActorStreamOut", 0, (Func<int, EntityId>) SampEntities.GetActorId, true);
            builder.UseMiddleware<EntityMiddleware>("OnActorStreamOut", 1, (Func<int, EntityId>) SampEntities.GetPlayerId);
        }

        [Event]
        public void OnPlayerGiveDamageActor(Entity player, int damagedActorId, float amount, int weaponid, int bodypart)
        {
            // TODO fire new event for actors
        }
    }
}