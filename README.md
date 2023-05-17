각 계측기별 현재 데이터와 비교 후 자동 경보 발령 프로그램
  - 2023/04/27 Group List를 토대로 Alert List를 Object별 배열로 관리하며 작동
  - 2023/04/28 UI적용을 위해 View List를 별도로 관리
  - 2023/04/28 WEB (Apache)에서 CGI 기반 장비 경보 발령 할 수 있게 수정 (미완성)
  - 2023/05/02 Group List에는 ','를 구분기호로 Alert List의 Index를 관리하고 Alert List를 별도로 관리
  - 2023/05/03 Alert List별 View List에 넣어 UI 적용
  - 2023/05/04 Alert List에 별도로 현재 Lv와 ChkCount로 Level별 Count를 정해 Group에 현재 Lv를 적용하여 그룹 관리
  
최종)
  - ini File에서 Data 들어오는 Term과 해당 Level의 데이터가 지속적으로 들어와야 경보 발령할지(Term) 저장하여 관리
  - ChkCount를 사용하지 않고 10칸짜리 배열을 만들어 시간별 데이터 관리
  - 데이터 통계 프로그램을 현재 신뢰할 수 없어 1분 데이터를 받아와서 통계내어 활용
