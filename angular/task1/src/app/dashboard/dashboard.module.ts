import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DashBoardRoutingModule } from './dashboard.-routing.module';
import { MatCardModule } from '@angular/material/card';
import { HighchartsChartModule } from 'highcharts-angular';
import { UtilitiesModule } from '../utilities/utilities.module';

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    MatCardModule,
    HighchartsChartModule,
    UtilitiesModule,
  ],
  exports: [DashBoardRoutingModule],
})
export class DashboardModule {}
