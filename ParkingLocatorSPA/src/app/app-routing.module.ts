import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ResourcesComponent } from './resources/resources.component';
import { EventsComponent } from './events/events.component';


const routes: Routes = [
   { path: 'resources', component: ResourcesComponent,data: { Map: false } },
   { path: 'events', component: EventsComponent,data: { Map: false } },];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
