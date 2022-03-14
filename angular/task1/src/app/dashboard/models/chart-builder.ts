import * as Highcharts from 'highcharts';
import {
  AxisTypeValue,
  ChartOptions,
  Options,
  SeriesOptionsType,
} from 'highcharts';

export interface IChart {
  setTitleText(text: string): ISetTitleCharOptions;
  setTooltip(headerFormat: string, pointFormat: string): IChart;
}
export interface ISetTitleCharOptions {
  setXAxis(
    type: AxisTypeValue,
    title?: string,
    tickInterval?: number,
    range?: { from: number; to: number }
  ): ISetYAxis;
}

export interface ISetYAxis {
  setYAxis(
    type: AxisTypeValue,
    title?: string,
    tickInterval?: number,
    range?: { from: number; to: number }
  ): ISetLegend;
}

export interface ISetLegend {
  defineSeries(type: ChartOptionsType, name: string): IChartOptions;
}

export interface IChartOptions {
  addSeriesData(key: string, data: number, color: string): IChartOptions;
  setSeriesName(name: string): IChartOptions;
  clearData(): Options;
  getOptions(): Options;
}

export class ChartBuilder
  implements
    IChart,
    ISetTitleCharOptions,
    ISetLegend,
    IChartOptions,
    ISetYAxis {
  private _options: Options;

  private constructor(type: ChartOptionsType) {
    this._options = {
      chart: {
        type: type,
      },
      accessibility: {
        announceNewData: {
          enabled: true,
        },
      },
      plotOptions: {
        bar: {
          allowPointSelect: true,
          cursor: 'pointer',
          dataLabels: {
            enabled: true,
            animation: true,
            shadow: true,
          },
          showInLegend: true,
          colorByPoint: true,
        },
      },

      drilldown: {
        series: [],
      },
      colors: [],
    };
  }
  setSeriesName(name: string): IChartOptions {
    (this._options.series as any)[0].name = name;
    return this;
  }

  setYAxis(
    type: AxisTypeValue,
    title?: string,
    tickInterval?: number,
    range?: { from: number; to: number }
  ): ISetLegend {
    this._options.yAxis = {
      type: type,
      title: {
        text: title,
      },
      minTickInterval: tickInterval,
    };
    if (range)
      this._options.yAxis.accessibility = {
        rangeDescription: `Range ${range.from} to ${range.to}`,
      };
    return this;
  }
  setTooltip(headerFormat: string, pointFormat: string): IChart {
    this._options.tooltip = {
      headerFormat: headerFormat,
      pointFormat: pointFormat,
    };
    return this;
  }
  setXAxis(
    type: AxisTypeValue,
    title?: string,
    tickInterval?: number,
    range?: { from: number; to: number }
  ): ISetYAxis {
    this._options.xAxis = {
      type: type,
      title: {
        text: title,
      },
      minTickInterval: tickInterval,
    };
    if (range)
      this._options.xAxis.accessibility = {
        rangeDescription: `Range ${range.from} to ${range.to}`,
      };

    return this;
  }
  clearData(): Options {
    (this._options.series as any)[0]['data'] = [];
    return this._options;
  }

  addSeriesData(key: string, data: number, color: string): IChartOptions {
    ((this._options.series as Array<SeriesOptionsType>)[0] as any)['data'].push(
      {
        name: key,
        y: data,
        color,
      }
    );
    return this;
  }
  getOptions(): Options {
    return this._options;
  }

  static GetChartBuilder(type: ChartOptionsType): IChart {
    return new ChartBuilder(type);
  }

  setTitleText(text: string): ISetTitleCharOptions {
    this._options.title = {
      text: text,
    };
    return this;
  }

  defineSeries(type: ChartOptionsType, name: string): IChartOptions {
    this._options.series = [
      {
        type: type,
        name: name,
        dataLabels: {
          enabled: true,
        },
        data: [],
      },
    ];

    return this;
  }
}

export type ChartOptionsType = 'bar' | 'column';
