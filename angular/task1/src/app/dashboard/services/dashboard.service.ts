import { Injectable } from '@angular/core';
import { BehaviorSubject, pipe, Subject } from 'rxjs';
import { HubService } from '../../utilities/services/hub.service';
import { FileRowModel } from '../models/file-row-model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { map, tap } from 'rxjs/operators';
import { ChartBuilder, IChartOptions } from '../models/chart-builder';
import { Options } from 'highcharts';
import { IFileModel } from '../../file-list/models/file-model';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private _chartOptions: IChartOptions;
  private onFetchedData = new Subject<any>();
  private _chartData: FileRowModel[] = [];
  isFetching$ = new Subject<boolean>();
  onOptionsChange = new BehaviorSubject<Options | null>(null);
  constructor(hubService: HubService, private httpClient: HttpClient) {
    this._chartOptions = this.buildChartConfig();
    this.onFetchedData
      .pipe(
        map((data) => {
          this.setChartData(data);
          return this._chartOptions.getOptions();
        }),
        tap((x) => this.isFetching$.next(false))
      )
      .subscribe(this.onOptionsChange);
    hubService.fileIsProcessed$.subscribe((x) => this.refreshData());
  }

  private setChartData(data: FileRowModel[]) {
    this._chartOptions.clearData();
    this.setSeriesData(data);
    if (data.some((x) => !!x)) {
      const lastDate = new Date(data.sort((a, b) => b.id - a.id)[0].insertTime);
      this._chartOptions.setSeriesName(
        `Last Uploaded Rows ${lastDate.toLocalizedDate()}`
      );
    }
  }

  private setSeriesData(data: any) {
    data.forEach((x: FileRowModel) => {
      this._chartOptions.addSeriesData(x.label, x.value, x.color);
    });
  }

  fetchData() {
    if (this._chartData.some((x) => !!x)) {
      this.onFetchedData.next(this._chartData);
      return;
    }
    this.refreshData();
  }

  private refreshData() {
    this.isFetching$.next(true);
    this.httpClient.get(environment.apis().getRows).subscribe((x) => {
      this.onFetchedData.next(x);
    });
  }

  private buildChartConfig() {
    return ChartBuilder.GetChartBuilder('bar')
      .setTooltip(
        '<span style="font-size:11px">{series.name}</span><br>',
        '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
      )

      .setTitleText('Valid Rows')
      .setXAxis('category', '')
      .setYAxis('linear', '')
      .defineSeries('bar', 'Last upload valid rows');
  }
}
