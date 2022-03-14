using AppLogic.Models;
using AutoMapper;
using Core.Entities;

namespace AppLogic;

public class MappingProfile : Profile
{
  public MappingProfile ()
  {
    CreateMap<UploadedFile, FileModel> ()
      .ForMember (x => x.DateOfUpload, x => x.MapFrom (c => c.InsertTime))
      .ForMember (x => x.FileStatusValue, x => x.MapFrom (c => c.FileStatus))
      .ForMember (x => x.FileStatus, x => x.MapFrom (c => Enum.GetName (typeof (UploadFileStatus), c.FileStatus)));

    CreateMap<FileRow, FileRowModel> ().ForMember (x => x.Value, x => x.MapFrom (c => c.Number));
  }


}