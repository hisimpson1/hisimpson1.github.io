# make_all_list_html_perfect.py  ← 이 이름으로 저장하세요!
import os
from datetime import datetime
from urllib.parse import quote

def sizeof_fmt(num, suffix='B'):
    for unit in [' ', 'K', 'M', 'G', 'T']:
        if abs(num) < 1024.0:
            return f"{num:3.1f}{unit}{suffix}".strip()
        num /= 1024.0
    return f"{num:.1f}Y{suffix}"

def generate_list_html(path, link_mode=True):
    name = os.path.basename(path) or "루트 폴더"
    parent = os.path.dirname(path)
    has_parent = os.path.isfile(os.path.join(parent, "list.html")) if path != os.path.abspath(os.sep) else False

    folders = []
    files = []

    try:
        items = os.listdir(path)
    except PermissionError:
        return

    for item in items:
        if item.startswith('.') or item == "list.html":
            continue
        full = os.path.join(path, item)
        if os.path.isdir(full):
            folders.append(item)
        elif os.path.isfile(full):
            try:
                size = os.path.getsize(full)
                mtime = datetime.fromtimestamp(os.path.getmtime(full)).strftime('%Y-%m-%d %H:%M')
                files.append((item, size, mtime))
            except:
                pass

    folders.sort(key=str.lower)
    files.sort(key=lambda x: x[0].lower())

    html = f"""<!DOCTYPE html>
<html lang="ko"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1">
<title>{name}</title>
<style>
    body{{font-family:'Malgun Gothic',sans-serif;margin:40px;background:#f8f9fa}}
    h1{{color:#2c3e50;border-bottom:4px solid #3498db;padding-bottom:8px;display:inline-block}}
    p{{color:#444;font-size:1.15em;margin:15px 0}}
    table{{width:100%;border-collapse:collapse;background:#fff;box-shadow:0 6px 25px rgba(0,0,0,.12);border-radius:12px;overflow:hidden}}
    th{{background:#3498db;color:#fff;padding:18px}} td{{padding:15px 20px}}
    tr:hover{{background:#f0f8ff}} a{{text-decoration:none;color:inherit;display:block}}
    a:hover{{color:#e74c3c;text-decoration:underline}}
    .up{{color:#8e44ad;font-weight:bold;font-size:18px}}
    .folder{{color:#e67e22;font-weight:bold}}
    .file{{color:#2980b9}}
    .size{{text-align:right;white-space:nowrap}}
</style></head><body>
<h1>폴더 {name}</h1>
<p>폴더 {len(folders)}개 · 파일 {len(files)}개</p>
<table><thead><tr><th style="text-align:left">이름</th><th>크기</th><th>수정일</th></tr></thead><tbody>"""

    if has_parent:
        html += '<tr><td class="up"><a href="../list.html">상위 폴더 (..)</a></td><td>-</td><td>-</td></tr>'

    for folder in folders:
        sub_list_path = os.path.join(path, folder, "list.html")
        if link_mode and os.path.isfile(sub_list_path):
            link = f"{quote(folder)}/list.html"
            html += f'<tr><td class="folder"><a href="{link}">폴더 {folder}/</a></td><td>-</td><td>-</td></tr>'
        else:
            html += f'<tr><td class="folder">폴더 {folder}/</td><td>-</td><td>-</td></tr>'

    for name, size, mtime in files:
        html += f'<tr><td class="file"><a href="{quote(name)}">파일 {name}</a></td><td class="size">{sizeof_fmt(size)}</td><td>{mtime}</td></tr>'

    html += f"""</tbody></table><hr><small>
        {datetime.now().strftime('%Y년 %m월 %d일 %H:%M')} 자동 생성
    </small></body></html>"""

    try:
        with open(os.path.join(path, "list.html"), "w", encoding="utf-8") as f:
            f.write(html)
    except Exception as e:
        print(f"저장 실패: {path}")

# 메인 실행 — 2단계로 완벽 해결!
print("모든 폴더에 list.html 생성 시작 (완벽 연결 버전)\n")

root_dir = os.getcwd()
total = 0

# 1단계: 모든 폴더에 list.html 먼저 생성 (링크 없이도 파일 존재하게)
print("1단계: 모든 폴더에 list.html 파일 생성 중...")
for current, dirs, _ in os.walk(root_dir):
    dirs[:] = [d for d in dirs if not d.startswith('.') and d not in ['__pycache__', 'node_modules', '.git', '.venv']]
    rel = os.path.relpath(current, root_dir)
    print(f"   생성 → {rel if rel != '.' else '현재 폴더'}")
    generate_list_html(current, link_mode=False)  # 링크는 나중에
    total += 1

# 2단계: 다시 돌면서 이번엔 정확히 링크 걸기
print("\n2단계: 클릭 가능한 링크 완성 중...")
for current, dirs, _ in os.walk(root_dir):
    dirs[:] = [d for d in dirs if not d.startswith('.') and d not in ['__pycache__', 'node_modules', '.git', '.venv']]
    rel = os.path.relpath(current, root_dir)
    print(f"   연결 → {rel if rel != '.' else '현재 폴더'}")
    generate_list_html(current, link_mode=True)   # 이제 진짜 링크 걸림!

print(f"\n완료! 총 {total}개 폴더에 완벽한 list.html 생성됨")
print("이제 아무 list.html이나 열어보세요 → 모든 폴더 클릭해서 자유롭게 이동 가능합니다!")