import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { Subject } from 'rxjs';
import { distinctUntilChanged, filter, takeUntil } from 'rxjs/operators';
import { DashboardService } from '../../services/dashboard.service';
import { Options } from 'highcharts';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
  highcharts = Highcharts;
  options$ = this.dashBoardService.onOptionsChange.asObservable();

  private destroyer = new Subject<any>();
  options: Options | undefined;
  constructor(private dashBoardService: DashboardService) {}
  isFetching$ = this.dashBoardService.isFetching$.pipe(distinctUntilChanged());
  ngOnInit(): void {
    this.dashBoardService.fetchData();
    this.dashBoardService.onOptionsChange
      .pipe(
        distinctUntilChanged(),
        filter((x) => !!x),
        takeUntil(this.destroyer)
      )
      .subscribe((x) => {
        this.options = { ...x };
      });
  }

  ngOnDestroy(): void {
    this.destroyer.next();
    this.destroyer.complete();
  }
}
